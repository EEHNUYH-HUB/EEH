using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using EEH;

namespace EEH.Utils
{
    public class QueueAsync
    {
        private Queue<QueueAsyncTask> tasks = new Queue<QueueAsyncTask>();
        private QueueAsyncTask currentTask;
        private BackgroundWorker worker;
        private static Dictionary<int, QueueAsync> instanceList = new Dictionary<int, QueueAsync>();
        private QueueAsync()
        {
            worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += (sender,e) => 
            {
                if (currentTask.ExNotNull() && currentTask.RunHandler.ExNotNull())
                {
                    currentTask.RunHandler();
                }
            };
            worker.RunWorkerCompleted += (sender, e) => 
            {
                if (currentTask.ExNotNull() && currentTask.CompleteHandler.ExNotNull())
                {
                    currentTask.CompleteHandler(!e.Error.ExNotNull(), e.Error);
                }


                currentTask = GetTask();
                if (currentTask.ExNotNull())
                {
                    worker.RunWorkerAsync();
                }
            };
        }

        public static QueueAsync Instance
        {
            get
            {
                return GetInstance(0);
            }
        }
        private static object instanceLock = "lock";
        public static QueueAsync GetInstance(int id)
        {
            lock (instanceLock)
            {
                if (!instanceList.ContainsKey(id))
                {
                    instanceList.Add(id, new QueueAsync());
                }
                return instanceList[id];
            }
        }

        public void AddTask(Action runHandler)
        {
            AddTask(runHandler,null); 
        }

        public void AddTask(Action runHandler, Action<bool, Exception> completeHandler)
        {
            QueueAsyncTask qTask = new QueueAsyncTask();
            qTask.RunHandler = runHandler;
            qTask.CompleteHandler = completeHandler;
            
            AddTask(qTask);
        }

        private object isLock = "lock";

        void AddTask(QueueAsyncTask task)
        {
            lock (isLock)
            {
                tasks.Enqueue(task);
                if (currentTask == null && !worker.IsBusy)
                {
                    currentTask = GetTask();
                    worker.RunWorkerAsync();
                }
            }
        }

        private QueueAsyncTask GetTask()
        {
            if (tasks.Count > 0)
            {
                return tasks.Dequeue();
            }

            return null;
        }


    }

    internal class QueueAsyncTask
    {
        public Action<bool ,Exception> CompleteHandler { get; set; }
        public Action RunHandler { get; set; }
        

    }
}

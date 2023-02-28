<template>
      <g :transform="'translate(' + props.Item.Rect.CX + ',' + props.Item.Rect.CY + ')'">

            <g :transform="'translate(-8,-8)'" @click="OnJoinType(props.Item)">
                  <circle fill="white" r="8" cx="8" cy="8"></circle>
                  <template v-if="props.Item.JoinArrow == 'START'">
                        <path v-if="props.Item.ColumnSP.X < props.Item.ColumnEP.X"
                        d="M1 8a7 7 0 1 0 14 0A7 7 0 0 0 1 8zm15 0A8 8 0 1 1 0 8a8 8 0 0 1 16 0zm-4.5-.5a.5.5 0 0 1 0 1H5.707l2.147 2.146a.5.5 0 0 1-.708.708l-3-3a.5.5 0 0 1 0-.708l3-3a.5.5 0 1 1 .708.708L5.707 7.5H11.5z" />
                        <path v-else
                        d="M1 8a7 7 0 1 0 14 0A7 7 0 0 0 1 8zm15 0A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM4.5 7.5a.5.5 0 0 0 0 1h5.793l-2.147 2.146a.5.5 0 0 0 .708.708l3-3a.5.5 0 0 0 0-.708l-3-3a.5.5 0 1 0-.708.708L10.293 7.5H4.5z" />
                  </template>
                  <template v-else-if="props.Item.JoinArrow == 'END'">
                        <path v-if="props.Item.ColumnSP.X > props.Item.ColumnEP.X"
                        d="M1 8a7 7 0 1 0 14 0A7 7 0 0 0 1 8zm15 0A8 8 0 1 1 0 8a8 8 0 0 1 16 0zm-4.5-.5a.5.5 0 0 1 0 1H5.707l2.147 2.146a.5.5 0 0 1-.708.708l-3-3a.5.5 0 0 1 0-.708l3-3a.5.5 0 1 1 .708.708L5.707 7.5H11.5z" />
                        <path v-else
                        d="M1 8a7 7 0 1 0 14 0A7 7 0 0 0 1 8zm15 0A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM4.5 7.5a.5.5 0 0 0 0 1h5.793l-2.147 2.146a.5.5 0 0 0 .708.708l3-3a.5.5 0 0 0 0-.708l-3-3a.5.5 0 1 0-.708.708L10.293 7.5H4.5z" />
                  </template>
                  <template v-else>
                        <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z"/>
  <path d="M8 4a.5.5 0 0 1 .5.5v3h3a.5.5 0 0 1 0 1h-3v3a.5.5 0 0 1-1 0v-3h-3a.5.5 0 0 1 0-1h3v-3A.5.5 0 0 1 8 4z"/>
                  </template>

            </g>
            <path :d="props.Item.Path" :stroke="props.Item.StrokeColor" :fill="props.Item.FillColor" stroke-width="0.5"
                  :marker-end="props.Item.EndType" :marker-start="props.Item.StartType" />

            <path :d="props.Item.Path2" :stroke="props.Item.StrokeColor" :fill="props.Item.FillColor" stroke-width="0.5"
                  :marker-end="props.Item.EndType" :marker-start="props.Item.StartType" />

            <foreignObject v-if="!props.Picker.IsDown" :x="props.Item.ColumnSP.X" :y="props.Item.ColumnSP.Y" style="overflow:visible;"
                  :width="props.Item.ColumnSP.W">
                  <n-space>
                        <n-popselect v-model:value="props.Item.StartJoinColumn" :options="props.Item.StartObj.Columns" trigger="click"
                              size="small" scrollable>
                              <n-button style="background-color: white;">
                                    {{
                                          props.Item.StartJoinColumn ? props.Item.StartObj.Alias + '.' +
                                                props.Item.StartJoinColumn : '컬럼을 선택하세요'
                                    }}
                              </n-button>
                        </n-popselect>
                  </n-space>
            </foreignObject>
            <foreignObject  v-if="!props.Picker.IsDown" :x="props.Item.ColumnEP.X" :y="props.Item.ColumnEP.Y" style="overflow:visible;"
                  :width="props.Item.ColumnEP.W">
                  <n-space>
                        <n-popselect v-model:value="props.Item.EndJoinColumn" :options="props.Item.EndObj.Columns" trigger="click"
                              size="small" scrollable>
                              <n-button style="background-color: white;">
                                    {{
                                          props.Item.EndJoinColumn ? props.Item.EndObj.Alias + '.' + props.Item.EndJoinColumn :
                                                '컬럼을 선택하세요'
                                    }}
                              </n-button>
                        </n-popselect>
                  </n-space>
            </foreignObject>
      </g>
</template>
<script setup>

import { ref, defineProps } from 'vue'
const props = defineProps({ Item: { type: Object },Picker:{type:Object} })

const OnJoinType = (item) => {
      if (item.JoinArrow == "START") {
            item.JoinArrow = "END";
      }
      else if (item.JoinArrow == "END") {
            item.JoinArrow = "";
      }
      else {
            item.JoinArrow = "START";
      }
}
</script>

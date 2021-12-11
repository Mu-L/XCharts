/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class SingleAxisHander : AxisHandler<SingleAxis>
    {
        protected override Orient orient { get { return component.orient; } }

        public override void InitComponent()
        {
            InitXAxis(component);
        }

        public override void Update()
        {
            UpdateAxisMinMaxValue(component.index, component);
            UpdatePointerValue(component);
        }

        public override void DrawBase(VertexHelper vh)
        {
            DrawSingleAxisSplit(vh, component);
            DrawSingleAxisLine(vh, component);
            DrawSingleAxisTick(vh, component);
        }

        private void InitXAxis(SingleAxis axis)
        {
            var theme = chart.theme;
            var xAxisIndex = axis.index;
            axis.painter = chart.painter;
            axis.refreshComponent = delegate ()
            {
                axis.UpdateRuntimeData(chart.chartX,
                    chart.chartY,
                    chart.chartWidth,
                    chart.chartHeight);

                InitAxis(axis, null, chart, this,
                    axis.orient,
                    axis.context.x,
                    axis.context.y,
                    axis.context.width,
                    axis.context.height);
            };
            axis.refreshComponent();
        }

        internal override void UpdateAxisLabelText(Axis axis)
        {
            base.UpdateAxisLabelText(axis);
            if (axis.IsTime() || axis.IsValue())
            {
                for (int i = 0; i < axis.context.labelObjectList.Count; i++)
                {
                    var label = axis.context.labelObjectList[i];
                    if (label != null)
                    {
                        var pos = GetLabelPosition(0, i);
                        label.SetPosition(pos);
                        CheckValueLabelActive(component, i, label, pos);
                    }
                }
            }
        }

        protected override Vector3 GetLabelPosition(float scaleWid, int i)
        {
            return GetLabelPosition(i, component.orient, component, null,
                chart.theme.axis,
                scaleWid,
                component.context.x,
                component.context.y,
                component.context.width,
                component.context.height);
        }

        private void DrawSingleAxisSplit(VertexHelper vh, SingleAxis axis)
        {
            if (AxisHelper.NeedShowSplit(axis))
            {
                var dataZoom = chart.GetDataZoomOfAxis(axis);
                DrawAxisSplit(vh, axis, chart.theme.axis, dataZoom,
                    axis.orient,
                    axis.context.x,
                    axis.context.y,
                    axis.context.width,
                    axis.context.height);
            }
        }

        private void DrawSingleAxisTick(VertexHelper vh, SingleAxis axis)
        {
            if (AxisHelper.NeedShowSplit(axis))
            {
                var dataZoom = chart.GetDataZoomOfAxis(axis);
                DrawAxisTick(vh, axis, chart.theme.axis, dataZoom,
                    axis.orient,
                    axis.context.x,
                    axis.context.y,
                    axis.context.width);
            }
        }

        private void DrawSingleAxisLine(VertexHelper vh, SingleAxis axis)
        {
            if (axis.show && axis.axisLine.show)
            {
                var axisStartY = axis.context.y + axis.offset;
                DrawAxisLine(vh, axis,
                    chart.theme.axis,
                    axis.orient,
                    axis.context.x,
                    axisStartY,
                    axis.context.width);
            }
        }
    }
}
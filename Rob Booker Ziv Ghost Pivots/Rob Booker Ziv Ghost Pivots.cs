using cAlgo.API;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class RobBookerZivGhostPivots : Indicator
    {
        private Bars _shortTermBars, _mediumTermBars, _longTermBars;

        [Parameter("Short Term", DefaultValue = "Hour8", Group = "TimeFrames")]
        public TimeFrame ShortTermTimeFrame { get; set; }

        [Parameter("Medium Term", DefaultValue = "Daily", Group = "TimeFrames")]
        public TimeFrame MediumTermTimeFrame { get; set; }

        [Parameter("Long Term", DefaultValue = "Weekly", Group = "TimeFrames")]
        public TimeFrame LongTermTimeFrame { get; set; }

        [Output("Short Term Pivot", LineColor = "Red", PlotType = PlotType.DiscontinuousLine)]
        public IndicatorDataSeries ShortTermPivot { get; set; }

        [Output("Medium Term Pivot", LineColor = "Blue", PlotType = PlotType.DiscontinuousLine)]
        public IndicatorDataSeries MediumTermPivot { get; set; }

        [Output("Long Term Pivot", LineColor = "Yellow", PlotType = PlotType.DiscontinuousLine)]
        public IndicatorDataSeries LongTermPivot { get; set; }

        protected override void Initialize()
        {
            _shortTermBars = MarketData.GetBars(ShortTermTimeFrame);
            _mediumTermBars = MarketData.GetBars(MediumTermTimeFrame);
            _longTermBars = MarketData.GetBars(LongTermTimeFrame);
        }

        public override void Calculate(int index)
        {
            ShortTermPivot[index] = GetPivotPoint(_shortTermBars, index);
            MediumTermPivot[index] = GetPivotPoint(_mediumTermBars, index);
            LongTermPivot[index] = GetPivotPoint(_longTermBars, index);
        }

        private double GetPivotPoint(Bars bars, int index)
        {
            var barsIndex = bars.OpenTimes.GetIndexByTime(Bars.OpenTimes[index]);

            return (bars.HighPrices[barsIndex] + bars.LowPrices[barsIndex] + bars.ClosePrices[barsIndex]) / 3;
        }
    }
}
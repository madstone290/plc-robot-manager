﻿using PlcRobotManager.Core.Vendor.Mitsubishi.Ranges;
using PlcRobotManager.Core.Vendor.Mitsubishi.Readers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlcRobotManager.Core.Vendor.Mitsubishi.Gatherers
{
    /// <summary>
    /// 데이터를 모두 랜덤읽기로 수집한다.
    /// </summary>
    public class RandomDataGatherer : BaseGatherer
    {
    

        /// <summary>
        /// 디바이스 목록
        /// </summary>
        private readonly List<DeviceLabel> _deviceLabels = new List<DeviceLabel>();

        private readonly List<RandomRange> _randomRanges = new List<RandomRange>();

        /// <summary>
        /// 한번에 읽어올 디바이스 개수
        /// </summary>
        private int _readLength = 100;

        public RandomDataGatherer(IMitsubishiPlc plc, IEnumerable<DeviceLabel> deviceLabels) : base(plc)
        {
            _deviceLabels.AddRange(deviceLabels.OrderBy(x => x.AddressString));

            int taken = 0;
            while (taken < _deviceLabels.Count)
            {
                int toTake = Math.Min(_deviceLabels.Count - taken, ReadLength);
                _randomRanges.Add(new RandomRange(_deviceLabels.Skip(taken).Take(toTake)));

                taken += toTake;
            }
        }

        /// <summary>
        /// 한번에 읽어올 디바이스 개수
        /// </summary>
        public int ReadLength
        {
            get => _readLength;
            set
            {
                if (value < 1)
                    _readLength = 1;
                else
                    _readLength = value;
            }
        }

        public override IEnumerable<BlockRange> BlockRanges => Enumerable.Empty<BlockRange>();

        public override IEnumerable<RandomRange> RandomRanges => _randomRanges;

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace TomatoTracker
{
    /// <summary>
    /// Holds the enum TomatoType and dictionaries associated with it.
    /// </summary>
    internal class Enums
    {
        /// <summary>
        /// TomatoType enum declaration.
        /// </summary>
        internal enum TomatoType
        {
            Unknown,
            Tall,
            Low,
            Ample,
            Bush
        }

        /// <summary>
        /// Holds specific intervals (in days) for watering different types of tomato.
        /// </summary>
        internal Dictionary<Enums.TomatoType, int> wateringIntervals = new()
        {
            {Enums.TomatoType.Unknown, 5},
            {Enums.TomatoType.Tall, 3},
            {Enums.TomatoType.Low, 5},
            {Enums.TomatoType.Ample, 3},
            {Enums.TomatoType.Bush, 4}
        };

        /// <summary>
        /// Holds specific intervals (in days) for trellising different types of tomato.
        /// </summary>
        internal Dictionary<Enums.TomatoType, int> trellisIntervals = new()
        {
            {Enums.TomatoType.Unknown, 10},
            {Enums.TomatoType.Tall, 7},
            {Enums.TomatoType.Low, 12},
            {Enums.TomatoType.Ample, 15},
            {Enums.TomatoType.Bush, 14}
        };

        /// <summary>
        /// Holds specific growing times for different types of tomato.
        /// </summary>
        internal Dictionary<Enums.TomatoType, int> growthTime = new()
        {
            {Enums.TomatoType.Unknown, 105},
            {Enums.TomatoType.Tall, 120},
            {Enums.TomatoType.Low, 105},
            {Enums.TomatoType.Ample, 95},
            {Enums.TomatoType.Bush, 100}
        };

        /// <summary>
        /// Get property for wateringIntervals.
        /// </summary>
        internal Dictionary<Enums.TomatoType, int> WateringIntervals => wateringIntervals;

        /// <summary>
        /// Get property for trellisIntervals.
        /// </summary>
        internal Dictionary<Enums.TomatoType, int> TrellisIntervals => trellisIntervals;

        /// <summary>
        /// Get property for growthTime.
        /// </summary>
        internal Dictionary<Enums.TomatoType, int> GrowthTime => growthTime;
    }
}

using System;

namespace WheelLogMerge
{
    internal class Row
    {
        public DateTime Date { get; set; }
        public float Speed { get; set; }
        public float Voltage { get; set; }
        public float Current { get; set; }
        public float Power { get; set; }
        public int BatteryLevel { get; set; }
        public float Distance { get; set; }
        public int Temperature { get; set; }
    }
}
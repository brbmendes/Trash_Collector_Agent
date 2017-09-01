using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trash_Collector_Agent.src
{
    class Agent
    {
        /// <summary>
        /// Capacity agent internal trash
        /// </summary>
        Int32 capacityInternalTrash;

        /// <summary>
        /// Current agent internal trash
        /// </summary>
        Int32 currentInternalTrash;
        
        /// <summary>
        /// Capacity agent internal battery
        /// </summary>
        Int32 capacityInternalBattery;

        /// <summary>
        /// Current agent internal battery
        /// </summary>
        Int32 currentInternalBattery;

        /// <summary>
        /// Agent old position;
        /// </summary>
        public Dictionary<String, Int32> oldPosition;

        Int32 positionX { get; set; }
        Int32 positionY { get; set; }



        public Agent(Int32 internalTrash, Int32 internalBattery)
        {
            this.capacityInternalTrash = internalTrash;
            this.currentInternalTrash = internalTrash;
            this.capacityInternalBattery = internalBattery;
            this.currentInternalBattery = internalBattery;
            oldPosition = new Dictionary<String, Int32>();
        }

        public Int32 getX()
        {
            return this.positionX;
        }

        public Int32 getY()
        {
            return this.positionY;
        }

        public Int32 usedInternalTrash()
        {
            return this.currentInternalTrash;
        }

        public void cleanInternalTrash()
        {
            this.currentInternalTrash = this.capacityInternalTrash;
        }

        public Int32 usedInternalBattery()
        {
            return this.currentInternalBattery;
        }

        public void collectTrash()
        {
            this.currentInternalTrash--;
        }

        public void rechargeBattery()
        {
            this.currentInternalBattery = this.capacityInternalBattery;
        }

        public void set(Int32 x, Int32 y){
            this.positionX = x;
            this.positionY = y;
        }

        public void consumeBattery()
        {
            this.currentInternalBattery--;
        }

        public void showPosition()
        {
            Console.WriteLine(String.Format("[{0},{1}]", this.positionX, this.positionY));
        }
    }
}

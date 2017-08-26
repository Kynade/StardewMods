﻿using System;
using Pathoschild.Stardew.Automate.Framework;
using StardewValley;
using SObject = StardewValley.Object;

namespace Pathoschild.Stardew.Automate.Machines.Tiles
{
    /// <summary>A coop incubator that accepts eggs and spawns chickens.</summary>
    internal class CoopIncubatorMachine : GenericMachine
    {
        /*********
        ** Properties
        *********/
        /// <summary>The recipes to process.</summary>
        private Recipe[] Recipes;

        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="machine">The underlying machine.</param>
        public CoopIncubatorMachine(SObject machine) : base(machine)
        {
            int minutesUntilReady = Game1.player.professions.Contains(2) ? 18000 : 9000;

            this.Recipes = new Recipe[]
            {
                // egg => chicken
                new Recipe(
                    input: -5,
                    inputCount: 1,
                    output: item => new SObject(item.parentSheetIndex, 1, false, -1, 0),
                    minutes: minutesUntilReady/2
                ),

                // dinosaur egg => dinosaur
                new Recipe(
                    input: 107,
                    inputCount: 1,
                    output: item => new SObject(item.parentSheetIndex, 1, false, -1, 0),
                    minutes: minutesUntilReady
                )
            };

        }

        /// <summary>Get the machine's processing state.</summary>
        /// <remarks>The coop incubator never produces an object output so it is never done.</remarks>
        public override MachineState GetState()
        {
            if (this.Machine.heldObject == null)
                return MachineState.Empty;

            return MachineState.Processing;
        }

        /// <summary>Get the output item.</summary>
        /// <remarks>The coop incubator never produces an object output.</remarks>
        public override ITrackedStack GetOutput()
        {
            return null;
        }

        /// <summary>Pull items from the connected pipes.</summary>
        /// <param name="pipes">The connected IO pipes.</param>
        /// <returns>Returns whether the machine started processing an item.</returns>
        public override bool Pull(IPipe[] pipes)
        {
            bool processing = this.GenericPullRecipe(pipes, this.Recipes);

            if (processing)
            {
                int eggIndex = this.Machine.heldObject.parentSheetIndex;
                this.Machine.parentSheetIndex = eggIndex == 180 || eggIndex == 182 || eggIndex == 305 ? this.Machine.parentSheetIndex + 2 : this.Machine.parentSheetIndex + 1;
            }

            return processing;
        }
    }
}

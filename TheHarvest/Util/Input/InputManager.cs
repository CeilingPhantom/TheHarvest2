using System;
using System.Collections.Generic;
using Nez;

namespace TheHarvest.Util.Input
{
    public class InputManager : GlobalManager
    {
        static readonly Lazy<InputManager> lazy = new Lazy<InputManager>(() => new InputManager());
        public static InputManager Instance => lazy.Value;

        SortedDictionary<int, FastList<IInputable>> inputablePriorities = new SortedDictionary<int, FastList<IInputable>>(
            // sort desc
            Comparer<int>.Create((x, y) => x > y ? -1 : x < y ? 1 : 0)
        );

        Dictionary<int, bool> currFrameCanAcceptInput = new Dictionary<int, bool>();

        InputManager() : base()
        {}

        public void Initialize()
        {
            Core.RegisterGlobalManager(this);
        }

        public void Register(IInputable inputable, int priority)
        {
            if (!this.inputablePriorities.ContainsKey(priority))
            {
                this.inputablePriorities[priority] = new FastList<IInputable>();
            }
            this.inputablePriorities[priority].Add(inputable);
        }

        public bool CanAcceptInput(int priority)
        {
            bool r;
            this.currFrameCanAcceptInput.TryGetValue(priority, out r);
            return r;
        }

        public override void Update()
        {
            this.currFrameCanAcceptInput.Clear();
            foreach (var entry in inputablePriorities)
            {
                var priority = entry.Key;
                var inputables = entry.Value;
                for (var i = 0; i < inputables.Length; ++i)
                {
                    if (inputables[i].InputCollision())
                    {
                        this.currFrameCanAcceptInput[priority] = true;
                    }
                    if (this.currFrameCanAcceptInput.ContainsKey(priority) && 
                        this.currFrameCanAcceptInput[priority])
                    {
                        return;
                    }
                }
            }
        }
    }
}
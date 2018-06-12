﻿using System;
using System.Collections;
using System.Reflection;
using System.Threading;
using System.Collections.Generic;

namespace Paway.Forms
{
    internal class ProgressStates : IEnumerable
    {
        private List<ProgressState> _states = new List<ProgressState>();
        public static IntPtr False = (IntPtr)(-1);
        private int index;

        public void Add(ProgressState state)
        {
            lock (this._states)
            {
                this._states.Add(state);
            }
        }
        public void Clear()
        {
            lock (this._states)
            {
                this._states.Clear();
            }
        }
        public void Remove(ProgressState state)
        {
            lock (this._states)
            {
                int index = this._states.IndexOf(state);
                if (index >= 0)
                {
                    for (int i = this._states.Count - 1; i >= index; i--)
                    {
                        this._states.RemoveAt(i);
                    }
                }
            }
        }

        public int Count
        {
            get
            {
                lock (this._states)
                {
                    return this._states.Count;
                }
            }
        }
        public ProgressState CurrentState
        {
            get
            {
                lock (this._states)
                {
                    if (this._states.Count < 1) return null;
                    return this._states[this._states.Count - 1];
                }
            }
        }
        public IntPtr CurrentHandle
        {
            get
            {
                lock (this._states)
                {
                    if (this._states.Count < 1) return IntPtr.Zero;
                    return this._states[this._states.Count - 1].Handle;
                }
            }
        }
        public string CurrentCaption
        {
            get
            {
                lock (this._states)
                {
                    if (this._states.Count < 1) return "Loading..";
                    return this._states[this._states.Count - 1].Caption;
                }
            }
        }
        public int Max
        {
            get
            {
                lock (this._states)
                {
                    if (this._states.Count < 1) return 100;
                    return this._states[this._states.Count - 1].Max;
                }
            }
        }
        public int Value
        {
            get
            {
                lock (this._states)
                {
                    if (this._states.Count < 1) return 50;
                    var state = this._states[this._states.Count - 1];
                    if (state.NoValue)
                        state.Value++;
                    if (state.Value > state.Max)
                    {
                        index++;
                        if (index < state.Max * 12 / 100)
                        {
                            state.Value = state.Max;
                        }
                        else
                        {
                            index = 0;
                            state.Value = 1;
                        }
                    }
                    return state.Value;
                }
            }
        }
        public IEnumerator GetEnumerator()
        {
            return this._states.GetEnumerator();
        }
    }
}


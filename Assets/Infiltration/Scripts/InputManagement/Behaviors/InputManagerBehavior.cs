using UnityEngine;

using System;

namespace InputManagement.Behaviors
{
	[Serializable]
	public abstract class InputManagerBehavior
	{
		public abstract string[] Tags { get; }
		public virtual void LeftClick(RaycastHit hit) {}
		public virtual void RightClick(RaycastHit hit) {}
		public virtual void Hover(RaycastHit hit) {}
	}
}
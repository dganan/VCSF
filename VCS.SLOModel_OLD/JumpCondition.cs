using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCS.SLOModel_OLD
{
	public abstract class JumpCondition<T> : SLOElement
	{
		public Scene SceneToJump { get; set; }

		public abstract bool Jump(T condition);
	}
}

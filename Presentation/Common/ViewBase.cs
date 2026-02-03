using Godot;

namespace TrucoProject.Presentation {


	public abstract partial class ViewBase : Node {
		protected void RunOnMainThread(string methodName, params Variant[] args) {
			CallDeferred(methodName, args);
		}
	}

	
}

using Godot;

namespace TrucoProject.Presentation {
	public abstract partial class ViewBase : Control {
		protected void RunOnMainThread(string methodName, params Variant[] args) {
			CallDeferred(methodName, args);
		}
	}
}

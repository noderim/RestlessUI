using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
#if HAS_LEAN_GUI

#else
using Lean.Transition;
using CW.Common;

namespace Lean.Gui
{
	/// <summary>This component allows you to make a UI element that can switch between two states.
	/// For example, you could create a panel that shows and hides using this component.
	/// NOTE: If you want to create windows that toggle open and closed, then I recommend you instead use the Lean Window component. This works identically, but it also registers with the Lean Window Closer component, allowing you to close them in sequence.</summary>
	[ExecuteInEditMode]
	[AddComponentMenu(LeanGui.ComponentMenuPrefix + "Hoverable Toggle")]
	public class LeanHoverableToggle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		/// <summary>This stores all active and enabled LeanToggle instances.</summary>
		public static LinkedList<LeanHoverableToggle> Instances = new LinkedList<LeanHoverableToggle>();

		/// <summary>This lets you change the current toggle state of this UI element.</summary>
		public bool On { set { Set(value); } get { return on; } } [SerializeField] private bool on;

		/// <summary>If you enable this, then any sibling GameObjects (i.e. same parent GameObject) will automatically be turned off.
		/// This allows you to make radio boxes, or force only one panel to show at a time, etc.</summary>
		public bool TurnOffSiblings { set { if (turnOffSiblings = value) TurnOffSiblingsNow(); } get { return turnOffSiblings; } } [SerializeField] private bool turnOffSiblings;

		/// <summary>This allows you to perform a transition when this toggle turns on.
		/// You can create a new transition GameObject by right clicking the transition name, and selecting <b>Create</b>.
		/// For example, the <b>Graphic.color Transition (LeanGraphicColor)</b> component can be used to change the color.
		/// NOTE: Any transitions you perform here should be reverted in the <b>Off Transitions</b> setting using a matching transition component.</summary>
		public LeanPlayer OnTransitions { get { if (onTransitions == null) onTransitions = new LeanPlayer(); return onTransitions; } } [SerializeField] private LeanPlayer onTransitions;

		/// <summary>This allows you to perform a transition when this toggle turns off.
		/// You can create a new transition GameObject by right clicking the transition name, and selecting <b>Create</b>.
		/// For example, the <b>Graphic.color Transition (LeanGraphicColor)</b> component can be used to change the color.</summary>
		public LeanPlayer OffTransitions { get { if (offTransitions == null) offTransitions = new LeanPlayer(); return offTransitions; } } [SerializeField] private LeanPlayer offTransitions;

		/// <summary>This allows you to perform an action when this toggle turns on.</summary>
		public UnityEvent OnOn { get { if (onOn == null) onOn = new UnityEvent(); return onOn; } } [SerializeField] private UnityEvent onOn;

		/// <summary>This allows you to perform an action when this toggle turns off.</summary>
		public UnityEvent OnOff { get { if (onOff == null) onOff = new UnityEvent(); return onOff; } } [SerializeField] private UnityEvent onOff;

		[System.NonSerialized]
		private LinkedListNode<LeanHoverableToggle> link;

        [Header("Hoverable Transitions")]
        public bool EnableHoverWhenOn { get { return enableHoverWhenOn; } } [SerializeField] private bool enableHoverWhenOn;

		public bool EnableHoverWhenOff { get { return enableHoverWhenOff; } } [SerializeField] private bool enableHoverWhenOff;

		public LeanPlayer OnEnterTransitions { get { if (onEnterTransitions == null) onEnterTransitions = new LeanPlayer(); return onEnterTransitions; } } [SerializeField] private LeanPlayer onEnterTransitions;

		public LeanPlayer OnExitTransitions { get { if (onExitTransitions == null) onExitTransitions = new LeanPlayer(); return onExitTransitions; } } [SerializeField] private LeanPlayer onExitTransitions;


		public LeanPlayer OffEnterTransitions { get { if (offEnterTransitions == null) offEnterTransitions = new LeanPlayer(); return offEnterTransitions; } } [SerializeField] private LeanPlayer offEnterTransitions;

		public LeanPlayer OffExitTransitions { get { if (offExitTransitions == null) offExitTransitions = new LeanPlayer(); return offExitTransitions; } } [SerializeField] private LeanPlayer offExitTransitions;
        

		/// <summary>This allows you to perform an action when the mouse/finger enters this UI element.</summary>
		public UnityEvent OnEnter { get { if (onEnter == null) onEnter = new UnityEvent(); return onEnter; } } [SerializeField] private UnityEvent onEnter;

		/// <summary>This allows you to perform an action when the mouse/finger exits this UI element.</summary>
		public UnityEvent OnExit { get { if (onExit == null) onExit = new UnityEvent(); return onExit; } } [SerializeField] private UnityEvent onExit;

		/// <summary>Track the currently down pointers so we can toggle the transition.</summary>
		[System.NonSerialized]
		private List<int> enteredPointers = new List<int>();

		public void OnPointerEnter(PointerEventData eventData)
		{
            if(on && enableHoverWhenOn)
            {
                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    enteredPointers.Add(eventData.pointerId);

                    if (onEnterTransitions != null)
                    {
                        onEnterTransitions.Begin();
                    }


                    if (onEnter != null)
                    {
                        onEnter.Invoke();
                    }
                }
            }
            else if(!on && enableHoverWhenOff)
            {
                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    enteredPointers.Add(eventData.pointerId);

                    if (offEnterTransitions != null)
                    {
                        offEnterTransitions.Begin();
                    }


                    if (onEnter != null)
                    {
                        onEnter.Invoke();
                    }
                }
            }

		}

		public void OnPointerExit(PointerEventData eventData)
		{
            if(on && enableHoverWhenOn)
            {
                if (enteredPointers.Remove(eventData.pointerId) == true)
                {
                    if (onExitTransitions != null)
                    {
                        onExitTransitions.Begin();
                    }

                    if (onExit != null)
                    {
                        onExit.Invoke();
                    }
                }
            }
            else if(!on && enableHoverWhenOff)
            {
                if (enteredPointers.Remove(eventData.pointerId) == true)
                {
                    if (offExitTransitions != null)
                    {
                        offExitTransitions.Begin();
                    }

                    if (onExit != null)
                    {
                        onExit.Invoke();
                    }
                }
            }
		}
		public void Set(bool value)
		{
			if (value == true)
			{
				TurnOn();
			}
			else
			{
				TurnOff();
			}
		}

		/// <summary>This allows you to toggle the state of this toggle (i.e. if it's turned on, then this will turn it off).</summary>
		[ContextMenu("Toggle")]
		public void Toggle()
		{
			On = !On;
		}

		/// <summary>If this toggle is turned off, then this will turn it on.</summary>
		[ContextMenu("Turn On")]
		public void TurnOn()
		{
			if (on == false)
			{
				on = true;

				TurnOnNow();
			}
		}

		[ContextMenu("Turn Off")]
		public void TurnOff()
		{
			if (on == true)
			{
				on = false;

				TurnOffNow();
			}
		}

		/// <summary>This will search for any sibling toggles (i.e. they have the same parent GameObject), and turn them off.</summary>
		[ContextMenu("Turn Off Siblings Now")]
		public void TurnOffSiblingsNow()
		{
			var parent = transform.parent;

			if (parent != null)
			{
				var ignore = transform.GetSiblingIndex();

				for (var i = parent.childCount - 1; i >= 0; i--)
				{
					if (i != ignore)
					{
						var sibling = parent.GetChild(i).GetComponent<LeanHoverableToggle>();

						if (sibling != null)
						{
							sibling.TurnOff();
						}
					}
				}
			}
		}

		/// <summary>This will go through every toggle in the scene except this one, and turn them off.</summary>
		[ContextMenu("Turn Off Others Now")]
		public void TurnOffOthersNow()
		{
			var node = Instances.First;

			for (var i = Instances.Count - 1; i >= 0; i--)
			{
				var instance = node.Value;

				if (instance != this)
				{
					instance.TurnOff();
				}

				node = node.Next;
			}
		}

		/// <summary>This will return true if all the active and enabled toggle instances with the specified GameObject name are turned on.</summary>
		public static bool AllOn(string name)
		{
			var node = Instances.First;
			var on   = false;

			for (var i = Instances.Count - 1; i >= 0; i--)
			{
				var instance = node.Value;

				if (instance.name == name)
				{
					if (instance.on == false)
					{
						return false;
					}

					on = true;
				}

				node = node.Next;
			}

			return on;
		}

		/// <summary>This will return true if all the active and enabled toggle instances with the specified GameObject name are turned off.</summary>
		public static bool AllOff(string name)
		{
			var node = Instances.First;
			var off   = false;

			for (var i = Instances.Count - 1; i >= 0; i--)
			{
				var instance = node.Value;

				if (instance.name == name)
				{
					if (instance.on == true)
					{
						return false;
					}

					off = true;
				}

				node = node.Next;
			}

			return off;
		}

		/// <summary>This allows you to set the <b>On</b> state of all the active and enabled toggle instances with the specified GameObject name.</summary>
		public static void SetAll(string name, bool on)
		{
			var node = Instances.First;

			for (var i = Instances.Count - 1; i >= 0; i--)
			{
				var instance = node.Value;

				if (instance.name == name)
				{
					instance.On = on;
				}

				node = node.Next;
			}
		}

		/// <summary>This allows you to toggle the state (i.e. if it's turned on, then this will turn it off) of all active and enabled toggle instances with the specified GameObject name.</summary>
		public static void ToggleAll(string name)
		{
			var node = Instances.First;

			for (var i = Instances.Count - 1; i >= 0; i--)
			{
				var instance = node.Value;

				if (instance.name == name)
				{
					instance.Toggle();
				}

				node = node.Next;
			}
		}

		/// <summary>This allows you to turn on every active and enabled toggle instance with the specified GameObject name.</summary>
		public static void TurnOnAll(string name)
		{
			var node = Instances.First;

			for (var i = Instances.Count - 1; i >= 0; i--)
			{
				var instance = node.Value;

				if (instance.name == name)
				{
					instance.TurnOn();
				}

				node = node.Next;
			}
		}

		/// <summary>This allows you to turn off each active and enabled LeanToggle instance with the specified GameObject name.</summary>
		public static void TurnOffAll(string name)
		{
			var node = Instances.First;

			for (var i = Instances.Count - 1; i >= 0; i--)
			{
				var instance = node.Value;

				if (instance.name == name)
				{
					instance.TurnOff();
				}

				node = node.Next;
			}
		}

		protected virtual void TurnOnNow()
		{
			if (turnOffSiblings == true)
			{
				TurnOffSiblingsNow();
			}

			if (onTransitions != null)
			{
				onTransitions.Begin();
			}

			if (onOn != null)
			{
				onOn.Invoke();
			}
		}

		protected virtual void TurnOffNow()
		{
			if (offTransitions != null)
			{
				offTransitions.Begin();
			}

			if (onOff != null)
			{
				onOff.Invoke();
			}
		}

		protected virtual void OnEnable()
		{
			link = Instances.AddLast(this);
		}

		protected virtual void OnDisable()
		{
			Instances.Remove(link);

			link = null;
		}
	}
}

#if UNITY_EDITOR
namespace Lean.Gui.Editor
{
	using UnityEditor;
	using TARGET = LeanHoverableToggle;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class LeanToggle_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			if (Draw("on", "This lets you change the current toggle state of this UI element.") == true)
			{
				Each(tgts, t => t.On = serializedObject.FindProperty("on").boolValue, true);
			}

			if (Draw("turnOffSiblings", "If you enable this, then any sibling GameObjects (i.e. same parent GameObject) will automatically be turned off. This allows you to make radio boxes, or force only one panel to show at a time, etc.") == true)
			{
				Each(tgts, t => t.TurnOffSiblings = serializedObject.FindProperty("turnOffSiblings").boolValue, true);
			}

			Separator();

			Draw("onTransitions", "This allows you to perform a transition when this toggle turns on. You can create a new transition GameObject by right clicking the transition name, and selecting Create. For example, the Graphic.color Transition (LeanGraphicColor) component can be used to change the color.\n\nNOTE: Any transitions you perform here should be reverted in the <b>Off Transitions</b> setting using a matching transition component.");
			Draw("offTransitions", "This allows you to perform a transition when this toggle turns off. You can create a new transition GameObject by right clicking the transition name, and selecting Create. For example, the Graphic.color Transition (LeanGraphicColor) component can be used to change the color.");

			Separator();

			Draw("onOn");
			Draw("onOff");

			Separator();

			Draw("enableHoverWhenOn");
			Draw("enableHoverWhenOff");

			if (tgt.EnableHoverWhenOn == true)
			{
				Separator();

				Draw("onEnterTransitions", "This allows you to perform a transition when the mouse/finger enters this UI element. You can create a new transition GameObject by right clicking the transition name, and selecting Create. For example, the Graphic.color Transition (LeanGraphicColor) component can be used to change the color.\n\nNOTE: Any transitions you perform here must be reverted in the Exit Transitions setting using a matching transition component.");
				Draw("onExitTransitions", "This allows you to perform a transition when the mouse/finger exits this UI element. You can create a new transition GameObject by right clicking the transition name, and selecting Create. For example, the Graphic.color Transition (LeanGraphicColor) component can be used to change the color.");
			}

			if (tgt.EnableHoverWhenOff == true)
			{
				Separator();

				Draw("offEnterTransitions", "This allows you to perform a transition when the mouse/finger enters this UI element. You can create a new transition GameObject by right clicking the transition name, and selecting Create. For example, the Graphic.color Transition (LeanGraphicColor) component can be used to change the color.\n\nNOTE: Any transitions you perform here must be reverted in the Exit Transitions setting using a matching transition component.");
				Draw("offExitTransitions", "This allows you to perform a transition when the mouse/finger exits this UI element. You can create a new transition GameObject by right clicking the transition name, and selecting Create. For example, the Graphic.color Transition (LeanGraphicColor) component can be used to change the color.");
			}

			if (tgt.EnableHoverWhenOn == true || tgt.EnableHoverWhenOff == true)
			{
				Separator();

				Draw("onEnter", "This allows you to perform an action when this toggle turns on.");
				Draw("onExit", "This allows you to perform an action when this toggle turns off.");
			}
		}
	}
}
#endif // End if UNITY_EDITOR
#endif // END IF LEAN GUI
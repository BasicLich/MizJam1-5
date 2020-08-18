using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MizJam1.Utilities;

namespace MizJam1.UIComponents
{
    public class UIContainer : UIComponent
    {
        public UIContainer(Point position, Point size, bool keepCentered = true)
        {
            Position = position;
            Size = size;
            KeepCentered = keepCentered;
        }

        private UIComponent child;
        private Point paddingUpLeft;

        /// <summary>
        /// Whether to keep this container's child centered or not.
        /// </summary>
        /// <value><c>true</c> if keep centered; otherwise, <c>false</c>.</value>
        public bool KeepCentered { get; set; }
        public UIComponent Child => child;

        /// <summary>
        /// Sets the child of this component.
        /// </summary>
        /// <param name="child">Child.</param>
        public virtual void AddChild(UIComponent child)
        {
            this.child = child;
            child.SetParent(this);

            if (KeepCentered) CenterPadding();
        }

        /// <summary>
        /// Sets this component's scale, and its child's
        /// </summary>
        /// <param name="scale">Scale.</param>
        public override void SetScale(int scale)
        {
            base.SetScale(scale);

            child?.SetScale(scale);

            if (KeepCentered) CenterPadding();
        }

        /// <summary>
        /// Centers the padding of this container based on it's child.
        /// </summary>
        public virtual void CenterPadding()
        {
            if (child == null) return;

            SetPadding((Size - child.Size).Divide(2));
        }

        /// <summary>
        /// Sets the padding.
        /// </summary>
        /// <param name="padding">Padding.</param>
        public virtual void SetPadding(Point padding)
        {
            PaddingUpLeft = padding;

            if (child == null) return;

            child.Position = padding;
        }

        /// <summary>
        /// Gets the total padding size of this element.
        /// </summary>
        /// <value>The total padding.</value>
        public virtual Point TotalPadding => PaddingUpLeft + PaddingDownRight;

        /// <summary>
        /// Gets the amount of padding for the up and left directions.
        /// </summary>
        /// <value>The padding up left.</value>
        public virtual Point PaddingUpLeft { get => paddingUpLeft; protected set => paddingUpLeft = value; }
        /// <summary>
        /// Gets the amount of padding for the down and right directions.
        /// </summary>
        /// <value>The padding down right.</value>
        public virtual Point PaddingDownRight => Size - PaddingUpLeft - (child?.Size ?? Point.Zero);

        /// <summary>
        /// Selects this object and it's child.
        /// </summary>
        public override void Select()
        {
            base.Select();

            child?.Select();
        }

        /// <summary>
        /// Deselects this object and it's child.
        /// </summary>
        public override void Deselect()
        {
            base.Deselect();

            child?.Deselect();
        }

        /// <summary>
        /// Draws itself and it's child, if it exists.
        /// </summary>
        /// <param name="sb">Sprite batch object to draw with.</param>
        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

            child?.Draw(sb);
        }

        public override void Execute()
        {
            base.Execute();
            child?.Execute();
        }
    }
}

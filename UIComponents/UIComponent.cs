using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MizJam1.UIComponents.Commands;
using MizJam1.Utilities;

namespace MizJam1.UIComponents
{
    public abstract class UIComponent
    {
        /// <summary>
        /// The command associated with this UIComponent.
        /// </summary>
        private ICommand command;
        private Point size;
        private Point position;

        /// <summary>
        /// This element's parent, <see langword="null"/> if it has no parents.
        /// </summary>
        /// <value>The parent.</value>
        public UIContainer Parent { get; private set; }

        /// <summary>
        /// Sets this UIComponent's <paramref name="parent"/>, properly positions it and resizes it.
        /// </summary>
        /// <param name="parent">Parent.</param>
        public virtual void SetParent(UIContainer parent)
        {
            Parent = parent;

            Position = parent.PaddingUpLeft;
            Size = parent.Size - parent.TotalPadding;
        }

        /// <summary>
        /// Gets this component's X position relative to it's parent.
        /// </summary>
        /// <value>The x.</value>
        public virtual int X => Position.X;
        /// <summary>
        /// Gets this component's Y position relative to it's parent.
        /// </summary>
        /// <value>The y.</value>
        public virtual int Y => Position.Y;
        /// <summary>
        /// Gets this component's width.
        /// </summary>
        /// <value>The width.</value>
        public virtual int Width => Size.X;
        /// <summary>
        /// Gets this component's height.
        /// </summary>
        /// <value>The height.</value>
        public virtual int Height => Size.Y;

        /// <summary>
        /// This component's position, relative to it's parent if it exists.
        /// </summary>
        /// <value>The position.</value>
        public virtual Point Position { get => position; set => position = value; }
        /// <summary>
        /// This component's size.
        /// </summary>
        /// <value>The size.</value>
        public virtual Point Size { get => size.Multiply(Scale); set => size = value; }

        /// <summary>
        /// This UI Element's scale
        /// </summary>
        /// <value>The scale.</value>
        public virtual int Scale { get; private set; } = 1;

        /// <summary>
        /// Sets this element's scale
        /// </summary>
        /// <param name="scale">Scale.</param>
        public virtual void SetScale(int scale)
        {
            Scale = scale;
        }

        /// <summary>
        /// This component's absolute position on the screen.
        /// </summary>
        /// <value>The absolute position.</value>
        public virtual Point AbsolutePosition => Position + (Parent?.AbsolutePosition ?? Point.Zero);

        /// <summary>
        /// Selects this component.
        /// </summary>
        public virtual void Select() => Selected = true;

        /// <summary>
        /// Deselects this component.
        /// </summary>
        public virtual void Deselect() => Selected = false;

        /// <summary>
        /// Whether or not this component is selected.
        /// </summary>
        /// <value><c>true</c> if selected; otherwise, <c>false</c>.</value>
        public virtual bool Selected { get; protected set; }

        /// <summary>
        /// The background image of this component.
        /// </summary>
        /// <value>The background image.</value>
        public virtual Texture2D BackgroundImage { get; set; }

        /// <summary>
        /// This background image of this component while it is selected.
        /// </summary>
        /// <value>The selected background image.</value>
        public virtual Texture2D SelectedBackgroundImage { get; set; }

        /// <summary>
        /// If selected, returns the selected image, unless it doesn't exist,
        /// in which case it just returns the default one.
        /// </summary>
        /// <value>The current background image.</value>
        public virtual Texture2D CurrentBackgroundImage => Selected ? SelectedBackgroundImage ?? BackgroundImage : BackgroundImage;

        /// <summary>
        /// This element's mask color for it's background image. White shows the color as is.
        /// </summary>
        /// <value>The color.</value>
        public virtual Color BackgroundColor { get; set; } = Color.White;

        /// <summary>
        /// The color of this component while it is selected.
        /// </summary>
        /// <value>The background color when this element is selected.</value>
        public virtual Color? SelectedBackgroundColor { get; set; }

        /// <summary>
        /// If selected, returns the selected color, unless it doesn't exist,
        /// in which case it just returns the default one.
        /// </summary>
        /// <value>The current color.</value>
        public virtual Color CurrentBackgroundColor => Selected ? SelectedBackgroundColor ?? BackgroundColor : BackgroundColor;

        /// <summary>
        /// Takes care of drawing itself at it's absolute position.
        /// </summary>
        /// <param name="sb">Spritebatch to draw with</param>
        public virtual void Draw(SpriteBatch sb)
        {
            if (BackgroundImage == null) return;

            sb.Draw(CurrentBackgroundImage, new Rectangle(AbsolutePosition, Size), CurrentBackgroundColor);
        }

        /// <summary>
        /// Sets this UIComponent's command to the one given.
        /// </summary>
        /// <param name="command">Command.</param>
        public virtual void AddCommand(ICommand command)
        {
            this.command = command;
        }

        /// <summary>
        /// Executes this UI Component's command, if it exists.
        /// </summary>
        public virtual void Execute()
        {
            command?.Execute();
        }

        public bool Contains(Point pos)
        {
            return new Rectangle(AbsolutePosition, Size).Contains(pos);
        }
    }
}

using Discord;
using Discord.WebSocket;
using IsekaiTechnologies.ImagiTextedDiscordGUI.Brushes;
using IsekaiTechnologies.ImagiTextedDiscordGUI.DrawingContexts;
using IsekaiTechnologies.ImagiTextedDiscordGUI.Renderer;
using IsekaiTechnologies.ImagiTextedDiscordGUI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Discord.Pages
{
    public class CanvasPage : Page
    {
        public IDrawingContext DrawingContext { get; set; }
        public static Brush BorderHorizontal = CreateBorderBrush("═");
        public static Brush BorderVertical = CreateBorderBrush("║");
        public static Brush BorderCross = CreateBorderBrush("╬");
        public static Brush BorderDownRight = CreateBorderBrush("╔");
        public static Brush BorderDownLeft = CreateBorderBrush("╗");
        public static Brush BorderUpRight = CreateBorderBrush("╚");
        public static Brush BorderUpLeft = CreateBorderBrush("╝");
        public static Brush BorderVerticalRight = CreateBorderBrush("╠");
        public static Brush BorderVerticalLeft = CreateBorderBrush("╣");
        public static Brush BorderHorizontalDown = CreateBorderBrush("╦");
        public static Brush BorderHorizontalUp = CreateBorderBrush("╩");

        public static Dictionary<JointOptions, Brush> CornerTopLeftJoints = new Dictionary<JointOptions, Brush>()
        {
            [JointOptions.NoJoint] = BorderDownRight,
            [JointOptions.OppositeJoint] = BorderHorizontalDown,
            [JointOptions.PerpendicularJoint] = BorderVerticalRight,
            [JointOptions.AllJoint] = BorderCross,
        };
        public static Dictionary<JointOptions, Brush> CornerTopRightJoints = new Dictionary<JointOptions, Brush>()
        {
            [JointOptions.NoJoint] = BorderDownLeft,
            [JointOptions.OppositeJoint] = BorderHorizontalDown,
            [JointOptions.PerpendicularJoint] = BorderVerticalLeft,
            [JointOptions.AllJoint] = BorderCross,
        };
        public static Dictionary<JointOptions, Brush> CornerBottomLeftJoints = new Dictionary<JointOptions, Brush>()
        {
            [JointOptions.NoJoint] = BorderUpRight,
            [JointOptions.OppositeJoint] = BorderHorizontalUp,
            [JointOptions.PerpendicularJoint] = BorderVerticalRight,
            [JointOptions.AllJoint] = BorderCross,
        };
        public static Dictionary<JointOptions, Brush> CornerBottomRightJoints = new Dictionary<JointOptions, Brush>()
        {
            [JointOptions.NoJoint] = BorderUpLeft,
            [JointOptions.OppositeJoint] = BorderHorizontalUp,
            [JointOptions.PerpendicularJoint] = BorderVerticalLeft,
            [JointOptions.AllJoint] = BorderCross,
        };

        public static Brush CreateBorderBrush(string unicodeChar)
        {
            return Brush.FromBrushElement(new ConstantWidthTextBrushElement(unicodeChar, 11.34d, 11.34d));
        }

        public CanvasPage(IDrawingContext ctx)
        {
            DrawingContext = ctx;
            ctx.GetRenderer().ButtonAdded += CanvasPage_ButtonAdded;
        }

        public override async Task OnNavigatedTo(Page sourcePage, PageManager manager, object args)
        {
            await base.OnNavigatedTo(sourcePage, manager, args);
            Manager.Client.ReactionAdded += Client_ReactionAdded;
        }

        private async Task Client_ReactionAdded(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            if(AddedButtonEntry.Any(x => x.Key.AttachedMessage.Id == arg1.Id && x.Key.Content == arg3.Emote.Name))
            {
                var entry = AddedButtonEntry.First( x => x.Key.AttachedMessage.Id == arg1.Id && x.Key.Content == arg3.Emote.Name);
                await entry.Value(arg1, arg2, arg3);
            }
        }

        private void CanvasPage_ButtonAdded(object sender, ButtonAddedEventArgs e)
        {
            if (ButtonEntries.Any(x => x.pos == e.Position))
            {
                var btnEntry = ButtonEntries.FirstOrDefault(x => x.pos == e.Position);
                AddedButtonEntry.Add(e, btnEntry.onClick);
            }
        }

        public void DrawGroupControl(Rect2DDouble rect, Brush header = null, Thickness2DDouble strokePadding = null,
            JointOptions tl = 0, JointOptions tr = 0, JointOptions bl = 0, JointOptions br = 0)
        {
            strokePadding = strokePadding ?? new Thickness2DDouble();
            double strokeBoundWidth = BorderHorizontal.Width + strokePadding.Left + strokePadding.Right;
            double strokeBoundHeight = Math.Round(BorderHorizontal.Height + strokePadding.Top + strokePadding.Bottom);
            double colEndWidth = Math.Round(rect.Width % strokeBoundWidth, 2);
            colEndWidth = colEndWidth == 0 ? strokeBoundWidth : colEndWidth;
            int rowEndHeight = (int)Math.Round(rect.Height % strokeBoundHeight);
            rowEndHeight = rowEndHeight == 0 ? (int)Math.Round(strokeBoundHeight) : rowEndHeight;
            DrawingContext.DrawLine(
                rect.Position.Offset(strokeBoundWidth, 0),
                rect.Position.Offset(rect.Width - colEndWidth, strokeBoundHeight),
                BorderHorizontal,
                strokePadding);
            DrawingContext.DrawLine(
                rect.Position.Offset(rect.Width - colEndWidth, strokeBoundHeight),
                rect.EndPosition.Offset(0d, -rowEndHeight),
                BorderVertical,
                new Thickness2DDouble(strokePadding.Left, strokePadding.Top, Math.Min(strokePadding.Right, colEndWidth - strokeBoundWidth), strokePadding.Bottom));
            DrawingContext.DrawLine(
                rect.Position.Offset(strokeBoundWidth, rect.Height - rowEndHeight),
                rect.EndPosition.Offset(-strokeBoundWidth, 0),
                BorderHorizontal,
                new Thickness2DDouble(strokePadding.Left, strokePadding.Top, strokePadding.Right, Math.Min(strokePadding.Bottom, rowEndHeight - strokeBoundHeight)));
            DrawingContext.DrawLine(
                rect.Position.Offset(0d, strokeBoundHeight),
                rect.Position.Offset(strokeBoundWidth, rect.Height - rowEndHeight),
                BorderVertical,
                strokePadding);

            // Corners
            var clip = new Rect2DDouble(0, 0, 11.34d, 11.34d);
            DrawingContext.DrawPoint(rect.Position, CornerTopLeftJoints[tl], clip);
            DrawingContext.DrawPoint(rect.Position.Offset(rect.Width - colEndWidth, 0), CornerTopRightJoints[tr], clip);
            DrawingContext.DrawPoint(rect.Position.Offset(0d, rect.Height - rowEndHeight), CornerBottomLeftJoints[bl], clip);
            DrawingContext.DrawPoint(rect.EndPosition.Offset(-strokeBoundWidth, -strokeBoundHeight), CornerBottomRightJoints[br], clip);

            // Header
            if (header != null)
            {
                DrawingContext.DrawPoint(rect.Position.Offset(strokeBoundWidth, 0), header, new Rect2DDouble(0, 0, header.Width, header.Height));
            }
        }

        protected void AddReactionButton(Position2DInt pos, string content, Func<Cacheable<IUserMessage, ulong>, ISocketMessageChannel, SocketReaction, Task> onClick)
        {
            DrawingContext.DrawButton(pos, content);
            ButtonEntries.Add((pos, content, onClick));
        }

        protected List<(Position2DInt pos, string content, Func<Cacheable<IUserMessage, ulong>, ISocketMessageChannel, SocketReaction, Task> onClick)> ButtonEntries = new List<(Position2DInt pos, string content, Func<Cacheable<IUserMessage, ulong>, ISocketMessageChannel, SocketReaction, Task> onClick)>();
        protected Dictionary<ButtonAddedEventArgs, Func<Cacheable<IUserMessage, ulong>, ISocketMessageChannel, SocketReaction, Task>> AddedButtonEntry = new Dictionary<ButtonAddedEventArgs, Func<Cacheable<IUserMessage, ulong>, ISocketMessageChannel, SocketReaction, Task>>();
    }
}

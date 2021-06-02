using Discord;
using Discord.WebSocket;
using IsekaiTechnologies.FacCord.Core;
using IsekaiTechnologies.FacCord.Core.DAL;
using IsekaiTechnologies.FacCord.Core.DAL.EntityFramework.Repositories;
using IsekaiTechnologies.FacCord.Core.Mine;
using IsekaiTechnologies.FacCord.Core.Models;
using IsekaiTechnologies.FacCord.Core.Models.Equipments;
using IsekaiTechnologies.FacCord.Core.Models.Mines;
using IsekaiTechnologies.FacCord.Core.Providers;
using IsekaiTechnologies.FacCord.Core.StorageManagements;
using IsekaiTechnologies.FacCord.Discord.Utils;
using IsekaiTechnologies.ImagiTextedDiscordGUI;
using IsekaiTechnologies.ImagiTextedDiscordGUI.Brushes;
using IsekaiTechnologies.ImagiTextedDiscordGUI.DrawingContexts;
using IsekaiTechnologies.ImagiTextedDiscordGUI.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Discord.Pages
{
    public class MinePage : FaccordCanvasPage
    {
        const double LogPosX = 585;
        const double LogPosY = 6;
        const double LogWrapWidth = 375;

        const double InfoPosX = 585;
        const double InfoPosY = 15;

        const double MapPosX = 60;
        const double MapPosY = 7;
        // Five "M": 87.05px + Emote: 22px + em-space: 16 = 125 
        // Also there are 7 cells in this, the first cell start at 20px
        const double ResourceCellsWidth = 125.05;
        // Single cell = 11.34 * 88
        const double ScreenWidth = 997.92;
        // 50/88 (~0.56) for the left pane
        const double MinePannelWidth = 567;
        const double RightPanelsWidth = 430.92;

        private Profile _CurrentProfile;
        private IStorageManagement Storage;

        private string _RuptureChemicalEmoji;
        private string _OutOfBoundEmoji;

        private Dictionary<string, Core.Models.Direction> _ReactionToDirection;
        private string _MoveForwardEmoji;
        private string _MoveBackEmoji;
        private string _MoveLeftEmoji;
        private string _MoveRightEmoji;


        public Core.Models.Position2DInt ViewportOffset { get; set; } = new Core.Models.Position2DInt(0, 0);
        public Region MineRegion { get; set; }
        public Dimension MineDimension { get; set; }

        public MinePage(IDrawingContext ctx) : base(ctx)
        {
        }

        public override async Task OnNavigatedTo(Page sourcePage, PageManager manager, object args)
        {
            await base.OnNavigatedTo(sourcePage, manager, args);
            _RuptureChemicalEmoji = await UIEmoji.GetUIEmoji("Sprites.Generic.RuptureChemical", FallbackEmoji);
            _OutOfBoundEmoji = await UIEmoji.GetUIEmoji("Sprites.Generic.OutOfBounds", FallbackEmoji);

            
            _MoveForwardEmoji = await UIEmoji.GetUIEmoji("Mine.Buttons.Forward", FallbackEmoji);
            _MoveBackEmoji = await UIEmoji.GetUIEmoji("Mine.Buttons.Back", FallbackEmoji);
            _MoveLeftEmoji = await UIEmoji.GetUIEmoji("Mine.Buttons.Left", FallbackEmoji);
            _MoveRightEmoji = await UIEmoji.GetUIEmoji("Mine.Buttons.Right", FallbackEmoji);
            _ReactionToDirection = new Dictionary<string, Core.Models.Direction>()
            {
                [_MoveForwardEmoji] = Core.Models.Direction.North,
                [_MoveBackEmoji] = Core.Models.Direction.South,
                [_MoveLeftEmoji] = Core.Models.Direction.West,
                [_MoveRightEmoji] = Core.Models.Direction.East,
            };

            _CurrentProfile = await DataAccess.Profiles.GetActiveProfileForMineDiscoveryAsync(PlayerContext.CurrentPlayer);
            Storage = await Manager.CreateStorageManagement(_CurrentProfile);
            MineRegion = _CurrentProfile.SelectedRegion;
            MineDimension = _CurrentProfile.SelectedRegion.Dimension;
        }

        public override async Task LoadView(IMessageChannel channel)
        {
            await base.LoadView(channel);
            await UpdateScreen();
            await CreateButtons();
        }

        private async Task CreateButtons()
        {
            using(var drawKey = await DrawingContext.Begin())
            {
                AddReactionButton(new ImagiTextedDiscordGUI.Utils.Position2DInt(1, 29), _MoveForwardEmoji, btnMove_Click);
                AddReactionButton(new ImagiTextedDiscordGUI.Utils.Position2DInt(1, 31), _MoveBackEmoji, btnMove_Click);
                AddReactionButton(new ImagiTextedDiscordGUI.Utils.Position2DInt(0, 30), _MoveLeftEmoji, btnMove_Click);
                AddReactionButton(new ImagiTextedDiscordGUI.Utils.Position2DInt(2, 30), _MoveRightEmoji, btnMove_Click);
            }
        }

        

        private async Task btnMove_Click(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            await PlayerContext.MineController.MoveAsync(_ReactionToDirection[arg3.Emote.Name]);
            await UpdateScreen();
        }

        #region Update Mine Screen
        public async Task UpdateScreen()
        {
            ViewportOffset = PlayerContext.MiningContext.Position.Offset(-9, -9);   

            using (var drawKey = await DrawingContext.Begin())
            {
                DrawBorders();
                await DrawResources();
                await DrawMap();
                await DrawInfo();
                await DrawLog();
            }
        }

        public void DrawBorders()
        {
            DrawGroupControl(
                new Rect2DDouble(0, 0, ScreenWidth, 5),
                Brush.FromBrushElement(TextBrushElement.FromSingleLineString(" Resources")));
            DrawGroupControl(
                new Rect2DDouble(0, 5, MinePannelWidth + 11.34, 24),
                Brush.FromBrushElement(TextBrushElement.FromSingleLineString(" Mine")));
            DrawGroupControl(
                new Rect2DDouble(MinePannelWidth, 5, RightPanelsWidth, 10),
                Brush.FromBrushElement(TextBrushElement.FromSingleLineString(" Log")),
                tl: JointOptions.OppositeJoint,
                bl: JointOptions.PerpendicularJoint);
            DrawGroupControl(
                new Rect2DDouble(MinePannelWidth, 14, RightPanelsWidth, 15),
                Brush.FromBrushElement(TextBrushElement.FromSingleLineString(" Info")),
                tl: JointOptions.PerpendicularJoint,
                tr: JointOptions.PerpendicularJoint,
                bl: JointOptions.OppositeJoint);
        }

        // TODO: Improve the layout for it to be centered but for proof of concept this will do
        public async Task DrawResources()
        {
            int count = 0;
            int rowIndex = 0;
            int columnIndex = 0;

            int matTypeCount = PlayerContext.LastDiscoveredMine.MaterialCounts.Count;
            foreach (var item in PlayerContext.LastDiscoveredMine.MaterialCounts.Take(matTypeCount <= 21 ? 21 : 20))
            {
                if (columnIndex == 7)
                {
                    rowIndex++;
                    columnIndex = 0;
                }
                string discEmoji = await DataAccess.EmojiLookups.GetBlockIcon(item.Key, FallbackEmoji);

                var brush = new Brush(new List<IBrushElement>()
                {
                    EmoteBrushElementHelper.GetBrushElementFromString(new string[] { discEmoji }),
                    TextBrushElement.FromSingleLineString($"{Resources.CreateSpace(16)}{item.Value}")
                });

                DrawingContext.DrawPoint(
                    new Position2DDouble(20 + columnIndex * ResourceCellsWidth, 1 + rowIndex),
                    brush,
                    new Rect2DDouble(0, 0, brush.Width, brush.Height));

                columnIndex++;
                count++;
            }
            
            if (matTypeCount > 21)
            {
                var br = Brush.FromBrushElement(TextBrushElement.FromSingleLineString("And many more...", FormattingOptions.Italic));
                DrawingContext.DrawPoint(
                    new Position2DDouble(20 + rowIndex * ResourceCellsWidth, 1 + columnIndex),
                    br,
                    new Rect2DDouble(0, 0, br.Width, br.Height));
            } 
        }

        public async Task DrawMap()
        {
            var mine = PlayerContext.LastDiscoveredMine;
            for (int h = 0; h < 20; h++)
            {
                List<IBrushElement> brushElements = new List<IBrushElement>();
                for (int w = 0; w < 20; w++)
                {
                    if (PlayerContext.MiningContext.Position == ViewportOffset.Offset(w,h))
                    {
                        brushElements.Add(EmoteBrushElementHelper.GetBrushElementFromString(new string[] { string.IsNullOrEmpty(_CurrentProfile.PlayerEmoji) ? FallbackEmoji : _CurrentProfile.PlayerEmoji }));
                    }
                    else
                    {
                        Core.Models.Position2DInt blockPos = ViewportOffset.Offset(w, h);

                        string discEmoji = await mine.IsValidPositionAsync(blockPos.X, blockPos.Y) ?
                            await DataAccess.EmojiLookups.GetBlockIcon(mine[blockPos], FallbackEmoji) :
                            _OutOfBoundEmoji ; 
                        brushElements.Add(EmoteBrushElementHelper.GetBrushElementFromString(new string[] { discEmoji }));
                    }
                }

                var brush = new Brush(brushElements);
                DrawingContext.DrawPoint(
                    new Position2DDouble(MapPosX, MapPosY + h),
                    brush,
                    new Rect2DDouble(0, 0, brush.Width, brush.Height));
            }
        }

        public async Task DrawInfo()
        {
            var mine = PlayerContext.LastDiscoveredMine;
            var mc = PlayerContext.MiningContext;

            string prospectorEmoji = string.Empty;
            string prospectorName;
            if (_CurrentProfile.CurrentProspector != null)
            {
                prospectorEmoji = await DataAccess.EmojiLookups.GetItemIcon(_CurrentProfile.CurrentProspector.Item, FallbackEmoji);
                prospectorName = _CurrentProfile.CurrentProspector.Item.Name;
            }
            else
            {
                prospectorName = "Unknown";
            }

            string drillEmoji = string.Empty;
            string drillName;
            if (_CurrentProfile.CurrentDrill != null)
            {
                drillEmoji = await DataAccess.EmojiLookups.GetItemIcon(_CurrentProfile.CurrentDrill.Item, FallbackEmoji);
                drillName = _CurrentProfile.CurrentDrill.Item.Name;
            }
            else
            {
                drillName = "Unknown";
            }

            TimeSpan tsRefreshDelta = _CurrentProfile.RuptureChemical.LastUpdated + _CurrentProfile.RuptureChemical.NextRefresh - DateTime.Now;
            TimeSpan tsRefresh = tsRefreshDelta > TimeSpan.Zero ? tsRefreshDelta : TimeSpan.Zero;

            var elements = new IBrushElement[]
            {
                TextBrushElement.FromSingleLineString("Mine Info", FormattingOptions.Italic | FormattingOptions.Underline),
                new NewLineBrushElement(1),
                TextBrushElement.FromSingleLineString("Mine size: "),
                TextBrushElement.FromSingleLineString($"{mine.Width} x {mine.Height}", FormattingOptions.Bold),
                new NewLineBrushElement(1),
                TextBrushElement.FromSingleLineString("Prospector: "),
                TextBrushElement.FromSingleLineString($"{prospectorEmoji} **{prospectorName}**"),
                new NewLineBrushElement(1),
                TextBrushElement.FromSingleLineString("Region: "),
                EmoteBrushElementHelper.GetBrushElementFromString(new string[] {await DataAccess.EmojiLookups.GetRegionIcon(MineRegion, FallbackEmoji) }),
                TextBrushElement.FromSingleLineString(MineRegion != null ? MineRegion.Name : "Not specified", FormattingOptions.Bold),
                new NewLineBrushElement(1),
                TextBrushElement.FromSingleLineString("Dimension: "),
                EmoteBrushElementHelper.GetBrushElementFromString(new string[] {await DataAccess.EmojiLookups.GetDimensionIcon(MineDimension, FallbackEmoji) }),
                TextBrushElement.FromSingleLineString(MineDimension != null ? MineDimension.Name : "Not specified", FormattingOptions.Bold),
                new NewLineBrushElement(2),
                TextBrushElement.FromSingleLineString("Player Info", FormattingOptions.Italic | FormattingOptions.Underline),
                new NewLineBrushElement(1),
                TextBrushElement.FromSingleLineString("Drill: "),
                TextBrushElement.FromSingleLineString($"{drillEmoji} **{drillName}**"),
                new NewLineBrushElement(1),
                TextBrushElement.FromSingleLineString("Rupture Chemical: "),
                TextBrushElement.FromSingleLineString($"{_RuptureChemicalEmoji} **{_CurrentProfile.RuptureChemical.Count}/{_CurrentProfile.RuptureChemical.Capacity} ({tsRefresh.FormatHourAndMinute()})**"),
                new NewLineBrushElement(1),
                TextBrushElement.FromSingleLineString("Health: ", FormattingOptions.Crossed),
                new NewLineBrushElement(1),
                TextBrushElement.FromSingleLineString("Hunger: ", FormattingOptions.Crossed),
                new NewLineBrushElement(1),
                TextBrushElement.FromSingleLineString("Storage: "),
                TextBrushElement.FromSingleLineString($"**{_CurrentProfile.Workshop.CurrentStorageItemCount}/{_CurrentProfile.Workshop.StorageCapacity}**")
            };

            var brush = new Brush(elements);
            DrawingContext.DrawPoint(
                new Position2DDouble(InfoPosX, InfoPosY),
                brush,
                new Rect2DDouble(0, 0, brush.Width, brush.Height));
        }

        // TODO: Might get logging info from log4net
        public async Task DrawLog()
        {
            var brush = new WrapBrush(new IBrushElement[] { TextBrushElement.FromSingleLineString("[21:18:00] [Info] This is the start of your log!") }, LogWrapWidth);
            DrawingContext.DrawPoint(
                new Position2DDouble(LogPosX, LogPosY),
                brush,
                new Rect2DDouble(0, 0, brush.Width, brush.Height));
        }
        #endregion
    }
}

using Data;
using EMM.Core.Converter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace EMM.Core.ViewModels
{
    public class SwipeViewModel : BaseViewModel, IActionViewModel, ILocationSettable
    {
        public SwipeViewModel(SimpleAutoMapper autoMapper, ViewModelFactory viewModelFactory)
        {
            this.autoMapper = autoMapper;
            this.viewModelFactory = viewModelFactory;

            InitializeCommands();
        }

        private SimpleAutoMapper autoMapper;
        private ViewModelFactory viewModelFactory;

        #region Public Properties

        /// <summary>
        /// This is a Swipe
        /// </summary>
        public BasicAction BasicAction { get => BasicAction.Swipe; set { } }

        /// <summary>
        /// Action's Description
        /// </summary>
        public string ActionDescription { get; set; } = "New Swipe";

        /// <summary>
        /// List of points for the swipe
        /// </summary>
        public List<SwipePoint> PointList { get; set; } = new List<SwipePoint>();

        /// <summary>
        /// List of points for the swipe. Use ObservableCollection to notify the view
        /// </summary>
        public ObservableCollection<SwipePointWrapper> PointCollection { get; set; } = new ObservableCollection<SwipePointWrapper>();

        /// <summary>
        /// Number of repeat swipe
        /// </summary>
        public int Repeat { get; set; } = 1;

        /// <summary>
        /// Wait time between each action
        /// </summary>
        public int WaitBetweenAction { get; set; } = 100;

        /// <summary>
        /// True to disable this action
        /// </summary>
        public bool IsDisable { get; set; }


        #endregion

        #region Commands

        public ICommand AddPointCommand { get; set; }

        public ICommand RemovePointCommand { get; set; }

        private void InitializeCommands()
        {
            AddPointCommand = new RelayCommand(p =>
            {
                var newSwipePoint = new SwipePoint();

                //set previous point
                if (PointCollection.Count > 0)
                {
                    var lastPoint = PointCollection.LastOrDefault();
                    newSwipePoint.Point = new Point(lastPoint.PointX, lastPoint.PointY);
                };

                var point = new SwipePointWrapper(newSwipePoint);
                this.PointCollection.Add(point);
            });

            RemovePointCommand = new RelayCommand(p =>
            {              
                this.PointCollection.RemoveAt(PointCollection.Count - 1);
            }, p => this.PointCollection.Count > 0);
        }

        #endregion

        #region Interface implement

        /// <summary>
        /// Convert the viewmodel back to model for saving, Generate scripts
        /// </summary>
        /// <returns></returns>
        public IAction ConvertBackToAction()
        {
            var swipe = this.autoMapper.SimpleAutoMap<SwipeViewModel, Swipe>(this);

            //Sync the changes
            swipe.PointList = new List<SwipePoint>(this.PointCollection.Select(i => new SwipePoint(i.SwipePoint)));

            return swipe;
        }

        public IActionViewModel MakeCopy()
        {
            var newSwipeVM = (SwipeViewModel)viewModelFactory.NewActionViewModel(this.BasicAction);
            this.autoMapper.SimpleAutoMap(this, newSwipeVM, new List<Type> { typeof(ICommand), typeof(ObservableCollection<SwipePointWrapper>), typeof(List<SwipePoint>) });
            newSwipeVM.PointCollection = new ObservableCollection<SwipePointWrapper>(this.PointCollection.Select(i => new SwipePointWrapper(new SwipePoint(i.SwipePoint))));
            newSwipeVM.PointList = new List<SwipePoint>(this.PointList.Select(p => new SwipePoint(p)));

            return newSwipeVM;
        }

        public IActionViewModel ConvertFromAction(IAction action)
        {
            this.autoMapper.SimpleAutoMap(action as Swipe, this);
            this.PointCollection = new ObservableCollection<SwipePointWrapper>((action as Swipe).PointList.Select(i => new SwipePointWrapper(i)));

            return this;
        }

        public IActionViewModel ChangeResolution(double scaleX, double scaleY, MidpointRounding roundMode = MidpointRounding.ToEven)
        {
            var newSwipe = this.MakeCopy() as SwipeViewModel;

            foreach (var swipePointWrapper in newSwipe.PointCollection)
            {
                swipePointWrapper.PointX = Math.Round(swipePointWrapper.PointX * scaleX, roundMode);
                swipePointWrapper.PointY = Math.Round(swipePointWrapper.PointY * scaleY, roundMode);
            }

            return newSwipe;
        }

        public void SetLocation(Point location)
        {
            this.PointCollection.Add(new SwipePointWrapper(new SwipePoint { Point = location }));
            IsChanged = true;
        }

        #endregion
    }

    /// <summary>
    /// Wrapper for <see cref="SwipePoint"/>
    /// </summary>
    public class SwipePointWrapper
    {
        public SwipePoint SwipePoint;

        public SwipePointWrapper(SwipePoint swipePoint)
        {
            SwipePoint = new SwipePoint(swipePoint);
        }

        /// <summary>
        /// X cordinate of point
        /// </summary>
        public double PointX
        {
            get
            {
                return SwipePoint.Point.X;
            }
            set
            {
                if (SwipePoint.Point.X == value)
                    return;

                SwipePoint.Point = new Point(value, SwipePoint.Point.Y);
            }
        }

        /// <summary>
        /// X cordinate of point
        /// </summary>
        public double PointY
        {
            get
            {
                return SwipePoint.Point.Y;
            }
            set
            {
                if (SwipePoint.Point.Y == value)
                    return;

                SwipePoint.Point = new Point(SwipePoint.Point.X, value);
            }
        }

        /// <summary>
        /// Hold at this point
        /// </summary>
        public int HoldTime
        {
            get
            {
                return SwipePoint.HoldTime;
            }
            set
            {
                SwipePoint.HoldTime = value;
            }
        }

        /// <summary>
        /// Speed of the swipe 1-50 from this point to the next
        /// </summary>
        public sbyte SwipeSpeed
        {
            get
            {
                return SwipePoint.SwipeSpeed;
            }
            set
            {
                SwipePoint.SwipeSpeed = value;
            }
        }

        public List<string> SpeedOptions { get; set; } = new List<string> {"Very Slow", "Slow", "Normal", "Fast", "Very Fast", "Flick"};
    }
}

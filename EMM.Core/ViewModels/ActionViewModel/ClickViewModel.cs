using Data;
using EMM.Core.Converter;
using System;
using System.Collections.Generic;
using System.Windows;

namespace EMM.Core.ViewModels
{
    public class ClickViewModel : BaseViewModel, IActionViewModel
    {
        public ClickViewModel(SimpleAutoMapper autoMapper, ViewModelFactory viewModelFactory)
        {
            this.autoMapper = autoMapper;
            this.viewModelFactory = viewModelFactory;
        }

        private SimpleAutoMapper autoMapper;
        private ViewModelFactory viewModelFactory;

        #region Public Properties

        /// <summary>
        /// This is a click
        /// </summary>
        public BasicAction BasicAction { get => BasicAction.Click; set { } }

        /// <summary>
        /// Action's Description
        /// </summary>
        public string ActionDescription { get; set; } = "New Click";

        /// <summary>
        /// The click location
        /// </summary>
        public Point ClickPoint { get; set; }

        /// <summary>
        /// Wrapper for ClickPoint
        /// </summary>
        public ClickPointWrapper PointWrapper { get; set; } = new ClickPointWrapper(new Point());

        /// <summary>
        /// 0 = click, > 0 = hold
        /// </summary>
        public int HoldTime { get; set; }

        /// <summary>
        /// Number of repeated click
        /// </summary>
        public int Repeat { get; set; } = 1;

        /// <summary>
        /// Wait time between each click
        /// </summary>
        public int WaitBetweenAction { get; set; } = 100;

        /// <summary>
        /// Convert the viewmodel back to model for saving, Generate scripts
        /// </summary>
        /// <returns></returns>
        public IAction ConvertBackToAction()
        {
            var click = this.autoMapper.SimpleAutoMap<ClickViewModel, Click>(this);
            //write change back to point struct
            click.ClickPoint = this.PointWrapper.point;
            return click;
        }

        public IActionViewModel ConvertFromAction(IAction action)
        {
            this.autoMapper.SimpleAutoMap<Click, ClickViewModel>(action as Click, this);
            //wrap point struct around pointVM
            this.PointWrapper.point = this.ClickPoint;
            return this;
        }

        public IActionViewModel MakeCopy()
        {
            var clickVM = (ClickViewModel)viewModelFactory.NewActionViewModel(this.BasicAction);
            this.autoMapper.SimpleAutoMap<ClickViewModel, ClickViewModel>(this, clickVM, new List<Type>() { typeof(ClickPointWrapper) });
            clickVM.PointWrapper.point = this.PointWrapper.point;
            return clickVM;
        }

        #endregion
    }

    /// <summary>
    /// Wrapper for <see cref="Point"/>
    /// </summary>
    public class ClickPointWrapper : BaseViewModel
    {
        public ClickPointWrapper(Point point)
        {
            this.point = point;
        }
        public Point point;

        public double X
        {
            get
            {
                return point.X;
            }
            set
            {
                if (point.X == value)
                    return;

                point = new Point(value, point.Y);
            }
        }

        public double Y
        {
            get
            {
                return point.Y;
            }
            set
            {
                if (point.Y == value)
                    return;

                point = new Point(point.X, value);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Windows;

namespace KTMath.Geometry.TwoD.SpatialSearch
{
    /// <summary>
    /// A 2D Grid Spatial Search data structure
    /// </summary>
    public class Grid2D
    {
        #region Geometry Container Class
        private class GeometryContainer : IGeometryContainer
        {
            public GeometryContainer(IGeometryContainer geometryContainer)
            {
                UnderlyingObject = geometryContainer;
            }

            public Rect Bounds => UnderlyingObject.Bounds;

            public IGeometryContainer UnderlyingObject { get; }

            /// <summary>
            /// Indicated whether this item was processed in current search iteration
            /// </summary>
            public bool Processed { get; set; }
        }

        #endregion

        #region Private fields

        private readonly Dictionary<Tuple<int, int>, List<GeometryContainer>> _geomsPerCell = new Dictionary<Tuple<int, int>, List<GeometryContainer>>();
        private readonly List<GeometryContainer> _processedList = new List<GeometryContainer>();
        private Rect _boundingRect;
        private int _gridResX;
        private int _gridResY;
        private double _cellSizeX;
        private double _cellSizeY;
        private bool _initialized;

        #endregion

        /// <summary>
        /// Clears the data structure
        /// </summary>
        public void Clear()
        {
            _boundingRect = Rect.Empty;
            _gridResX = 0;
            _gridResY = 0;
            _cellSizeX = 0.0;
            _cellSizeY = 0.0;
            _processedList.Clear();
            _geomsPerCell.Clear();
            _initialized = false;
        }

        /// <summary>
        /// Resets the data structure
        /// </summary>
        /// <param name="bounds">Bounds of all geometries that are about to enter into the data structure</param>
        /// <param name="gridResX">Horiziontal cell count</param>
        /// <param name="gridResY">Vertical cell count</param>
        public void Reset(Rect bounds, int gridResX, int gridResY)
        {
            _boundingRect = bounds;
            _gridResX = gridResX;
            _gridResY = gridResY;
            _cellSizeX = _boundingRect.Width / gridResX;
            _cellSizeY = _boundingRect.Height / gridResY;
            _processedList.Clear();
            _geomsPerCell.Clear();
            _initialized = true;
        }

        /// <summary>
        /// Adds geometry item into the data structure
        /// </summary>
        /// <param name="geom"></param>
        public void AddGeometry(IGeometryContainer geom)
        {
            if (!_initialized)
                return;
            if (geom == null)
                return;

            int cl, cr, ct, cb;
            GetOverlappingCells(geom.Bounds, out cl, out cr, out ct, out cb);
            var geomContainer = new GeometryContainer(geom);
            for (int j = cl; j <= cr; j++)
            for (int k = ct; k <= cb; k++)
            {
                var key = new Tuple<int, int>(j, k);
                List<GeometryContainer> listPerCell;
                var exists = _geomsPerCell.TryGetValue(key, out listPerCell);
                if (exists)
                    listPerCell.Add(geomContainer);
                else
                    _geomsPerCell.Add(key, new List<GeometryContainer> {  geomContainer });
            }
        }

        /// <summary>
        /// Removes geometry from the data structure
        /// </summary>
        /// <param name="geom"></param>
        public void RemoveGeometry(IGeometryContainer geom)
        {
            if (!_initialized)
                return;
            if (geom == null)
                return;

            int cl, cr, ct, cb;
            GetOverlappingCells(geom.Bounds, out cl, out cr, out ct, out cb);

            for (int j = cl; j <= cr; j++)
            for (int k = ct; k <= cb; k++)
            {
                var key = new Tuple<int, int>(j, k);
                List<GeometryContainer> listPerCell;
                var exists = _geomsPerCell.TryGetValue(key, out listPerCell);
                if (!exists)
                    continue;
                
                for (int i = 0; i < listPerCell.Count; i++)
                    if (ReferenceEquals(geom, listPerCell[i].UnderlyingObject))
                    {
                        listPerCell.RemoveAt(i);
                        break;
                    }
               
            }
        }

        /// <summary>
        /// Performs a search in the spatial data structure. All intersection candidates shall be passed
        /// to the IntersectionCandidateProcessor function object to be processed according to user logic
        /// </summary>
        /// <param name="candidateProcessor">Function object for processing intersection candidates</param>
        public void ProcessCandidates(IIntersectionCandidateProcessor candidateProcessor)
        {
            if (!_initialized)
                return;
            if (candidateProcessor?.GeometryToIntersect?.Bounds == null)
                return;
            int cl, cr, ct, cb;
            var targetBounds = candidateProcessor.GeometryToIntersect.Bounds;
            GetOverlappingCells(targetBounds, out cl, out cr, out ct, out cb);
            int cnt = 0;
            for (int j = cl; j <= cr; j++)
            for (int k = ct; k <= cb; k++)
            {
                var key = new Tuple<int, int>(j, k);
                List<GeometryContainer> listPerCell;
                var exists = _geomsPerCell.TryGetValue(key, out listPerCell);
                if (!exists)
                    continue;
                cnt += listPerCell.Count;
                for (int i = 0; i < listPerCell.Count; i++)
                {
                    var current = listPerCell[i];
                    if (current.Processed)
                        continue;
                    current.Processed = true;
                    _processedList.Add(current);
                    if (!current.Bounds.IntersectsWith(targetBounds))
                        continue;
                    candidateProcessor.ProcessCandidate(current.UnderlyingObject);  
                    if (candidateProcessor.ResultFound)
                        goto end; 
                         
                }
            }
            end:
            for (int i = 0; i < _processedList.Count; i++)
                _processedList[i].Processed = false;
            _processedList.Clear();
         
        }

        #region  Private methods

        /// <summary>
        /// Determines the cell range that overlap with the specified rectangle
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="cl"></param>
        /// <param name="cr"></param>
        /// <param name="ct"></param>
        /// <param name="cb"></param>
        private void GetOverlappingCells(Rect rect, out int cl, out int cr, out int ct, out int cb)
        {
            if (!_boundingRect.IntersectsWith(rect))
            {
                cl = cr = ct = cb = -1;
                return;
            }

            var d = rect.Left - _boundingRect.Left;
            if (d <= 0.0)
                cl = 0;
            else
                cl = (int)Math.Floor(d / _cellSizeX);

            d = rect.Right - _boundingRect.Left;
            if (d >= _boundingRect.Right)
                cr = _gridResX - 1;
            else
                cr = (int)Math.Floor(d / _cellSizeX);

            d = rect.Top - _boundingRect.Top;
            if (d <= 0.0)
                ct = 0;
            else
                ct = (int)Math.Floor(d / _cellSizeY);

            d = rect.Bottom - _boundingRect.Top;
            if (d >= _boundingRect.Bottom)
                cb = _gridResY - 1;
            else
                cb = (int)Math.Floor(d / _cellSizeY);

        }

        #endregion
    }
}

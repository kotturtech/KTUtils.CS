using System.Windows;

namespace KTMath.Geometry.TwoD.SpatialSearch
{

    /// <summary>
    /// Interface for geometry container for spatial data structures
    /// </summary>
    public interface IGeometryContainer
    {
        Rect Bounds { get; }
    }

    /// <summary>
    /// Function object for processing intersection candidates found by spatial search data structures
    /// </summary>
    public interface IIntersectionCandidateProcessor
    {
        void ProcessCandidate(IGeometryContainer geom);
        IGeometryContainer GeometryToIntersect { get; }

        bool ResultFound { get; }
    }

}

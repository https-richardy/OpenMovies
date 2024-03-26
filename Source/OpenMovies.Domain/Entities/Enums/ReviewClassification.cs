namespace OpenMovies.Domain.Enitites.Enums;

/// <summary>
/// Represents the classification of a movie review.
/// </summary>
/// This enumeration defines different levels of classification for movie reviews,
/// ranging from "Bad" to "Excellent", providing a scale for evaluating the quality
/// of a review.
/// </remarks>
public enum ReviewClassification
{
    /// <summary>
    /// Indicates a review with a negative assessment.
    /// </summary>
    /// <remarks>
    /// Indicating a low rating or unfavorable evaluation of the movie.
    /// </remarks>
    Bad,

    /// <summary>
    /// Indicates a review with an average assessment.
    /// </summary>
    /// <remarks>
    /// Indicating a middling rating or an assessment lacking strong positive
    /// or negative sentiment towards the movie.
    /// </remarks>
    Average,

    /// <summary>
    /// Indicates a review with a positive assessment.
    /// </summary>
    /// <remarks>
    /// Indicating a favorable rating or positive evaluation of the movie.
    /// </remarks>
    Good,

    /// <summary>
    /// Indicates an outstanding review assessment.
    /// </summary>
    /// <remarks>
    /// Indicating an outstanding rating or highly favorable evaluation of the movie.
    /// </remarks>
    Excellent
}
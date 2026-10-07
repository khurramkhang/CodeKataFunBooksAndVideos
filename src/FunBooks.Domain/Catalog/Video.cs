using FunBooks.Domain.Common;

namespace FunBooks.Domain.Catalog;


public sealed class Video : CatalogItem
{
    public Video(string id, string name, TimeSpan duration, Money price)
        : base(id, name, price)
    {
        if (duration <= TimeSpan.Zero)
        {
            throw new DomainRuleException("video.duration", "A video must have a positive duration.");
        }

        Duration = duration;
    }

    public TimeSpan Duration { get; }

    public override bool IsPhysical => false;

    public override ProductKind Kind => ProductKind.Video;
}

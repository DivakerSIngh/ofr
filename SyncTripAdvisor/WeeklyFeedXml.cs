using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SyncTripAdvisor
{
    [XmlRoot(ElementName = "Author")]
    public class Author
    {
        [XmlElement(ElementName = "AuthorName")]
        public string AuthorName { get; set; }
        [XmlElement(ElementName = "SmallAvatarURL")]
        public SmallAvatarURL SmallAvatarURL { get; set; }
        [XmlElement(ElementName = "LargeAvatarURL")]
        public LargeAvatarURL LargeAvatarURL { get; set; }
    }

    [XmlRoot(ElementName = "SmallAvatarURL")]
    public class SmallAvatarURL
    {
        [XmlAttribute(AttributeName = "height")]
        public string Height { get; set; }
        [XmlAttribute(AttributeName = "width")]
        public string Width { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "LargeAvatarURL")]
    public class LargeAvatarURL
    {
        [XmlAttribute(AttributeName = "height")]
        public string Height { get; set; }
        [XmlAttribute(AttributeName = "width")]
        public string Width { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "ManagementResponse")]
    public class ManagementResponse
    {
        [XmlElement(ElementName = "ModerationStatus")]
        public string ModerationStatus { get; set; }
        [XmlElement(ElementName = "DatePublished")]
        public string DatePublished { get; set; }
        [XmlElement(ElementName = "Language")]
        public string Language { get; set; }
        [XmlElement(ElementName = "Text")]
        public string Text { get; set; }
        [XmlElement(ElementName = "Author")]
        public Author Author { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "Review")]
    public class Review
    {
        [XmlElement(ElementName = "ModerationStatus")]
        public string ModerationStatus { get; set; }
        [XmlElement(ElementName = "DatePublished")]
        public string DatePublished { get; set; }
        [XmlElement(ElementName = "ReviewURL")]
        public string ReviewURL { get; set; }
        [XmlElement(ElementName = "Rating")]
        public string Rating { get; set; }
        [XmlElement(ElementName = "Language")]
        public string Language { get; set; }
        [XmlElement(ElementName = "Title")]
        public string Title { get; set; }
        [XmlElement(ElementName = "Text")]
        public string Text { get; set; }
        [XmlElement(ElementName = "Author")]
        public Author Author { get; set; }
        [XmlElement(ElementName = "TripType")]
        public string TripType { get; set; }
        [XmlElement(ElementName = "ManagementResponse")]
        public ManagementResponse ManagementResponse { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "Reviews")]
    public class Reviews
    {
        [XmlElement(ElementName = "Review")]
        public Review Review { get; set; }
    }

    [XmlRoot(ElementName = "Property")]
    public class Property
    {
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "Reviews")]
        public Reviews Reviews { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "cityid")]
        public string Cityid { get; set; }
        [XmlAttribute(AttributeName = "countryid")]
        public string Countryid { get; set; }
    }
}

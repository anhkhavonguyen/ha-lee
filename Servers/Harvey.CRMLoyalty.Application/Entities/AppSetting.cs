namespace Harvey.CRMLoyalty.Application.Entities
{
    public class AppSetting : EntityBase
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string GroupName { get; set; }
        public int AppSettingTypeId { get; set; }
        public string UserId { get; set; }
        public virtual AppSettingType AppSettingType { get; set; }
    }
}

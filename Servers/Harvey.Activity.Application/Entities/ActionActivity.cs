namespace Harvey.Activity.Application.Entities
{
    public class ActionActivity : EntityBase<string>
    {
        public string ActionTypeId { get; set; }
        public virtual ActionType ActionType { get; set; }
        public string ActionAreaId { get; set; }
        public virtual AreaActivity AreaActivity { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public string Value { get; set; }
    }
}

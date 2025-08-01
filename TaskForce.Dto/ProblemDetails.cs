namespace TaskForce.Dto
{
    public class ProblemDetails
    {
        public string Type { get; set; }

        public string Title { get; set; }

        public int? Status { get; set; }

        public string Detail { get; set; }

        public string Instance { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }
}

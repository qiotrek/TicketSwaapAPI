namespace TicketSwaapAPI.StoreModels
{
    public class FirestoreTableNamesConfig
    {
        private struct Defaults
        {
            internal static readonly string AnnouncementsTableName = "Announcements";
            internal static readonly string UsersTableName = "Users";
            internal static readonly string DeletedUsersTableName = "DeletedUsers";
            internal static readonly string NewActionsPropositionsTableName = "NewActionsPropositions";
            internal static readonly string ActiveActionsTableName = "ActiveActions";
            internal static readonly string OffertsTableName = "Offerts";
            internal static readonly string ProblemsAndQuestionsTableName = "ProblemsAndQuestions";
        }
        public string AnnouncementsTableName { get; set; }= Defaults.AnnouncementsTableName;
        public string UsersTableName { get; set; }= Defaults.UsersTableName;
        public string DeletedUsersTableName { get; set; }= Defaults.DeletedUsersTableName;
        public string NewActionsPropositionsTableName { get; set; }= Defaults.NewActionsPropositionsTableName;
        public string ActiveActionsTableName { get; set; }= Defaults.ActiveActionsTableName;
        public string OffertsTableName { get; set; }= Defaults.OffertsTableName;
        public string ProblemsAndQuestionsTableName { get; set; }= Defaults.ProblemsAndQuestionsTableName;

        public FirestoreTableNamesConfig WithPrefixAndSuffix(string pref,string suf)  {

            AnnouncementsTableName=pref+ AnnouncementsTableName+suf;
            UsersTableName = pref+ UsersTableName + suf;
            DeletedUsersTableName = pref+ DeletedUsersTableName + suf;
            NewActionsPropositionsTableName = pref+ NewActionsPropositionsTableName + suf;
            ActiveActionsTableName = pref+ ActiveActionsTableName + suf;
            OffertsTableName = pref+ OffertsTableName + suf;
            ProblemsAndQuestionsTableName = pref+ ProblemsAndQuestionsTableName + suf;

            return this;
        }
    }
}

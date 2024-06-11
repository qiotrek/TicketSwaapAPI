namespace TicketSwaapAPI.StoreModels
{
    public class FirestoreTableNamesConfig
    {
        private struct Defaults
        {
            internal static readonly string AnnouncementsTableName = "announcements";
            internal static readonly string UsersTableName = "users";
            internal static readonly string DeletedUsersTableName = "deletedUsers";
        }
        public string AnnouncementsTableName { get; set; }= Defaults.AnnouncementsTableName;
        public string UsersTableName { get; set; }= Defaults.UsersTableName;
        public string DeletedUsersTableName { get; set; }= Defaults.DeletedUsersTableName;

        public FirestoreTableNamesConfig WithPrefixAndSuffix(string pref,string suf)  {

            AnnouncementsTableName=pref+ AnnouncementsTableName+suf;
            UsersTableName = pref+ UsersTableName + suf;
            DeletedUsersTableName = pref+ DeletedUsersTableName + suf;

            return this;
        }
    }
}

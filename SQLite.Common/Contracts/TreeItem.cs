using Utility.Database;

namespace SQLite.Common.Contracts
{
    /// <summary>
    /// The base class for tree view items.
    /// </summary>
    public abstract class TreeItem
    {
        protected TreeItem(string displayName, ConnectionPath databasePath)
        {
            if (string.IsNullOrEmpty(displayName))
            {

            }
            DisplayName = displayName;
            DatabasePath = databasePath;
        }

        /// <summary>
        /// Gets or sets the name that is displayed in the tree view.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName { get; }


        /// <summary>
        /// Gets or sets the database path. This is used to determine the current database context when a tree item is clicked.
        /// </summary>
        /// <value>
        /// The database path.
        /// </value>
        public ConnectionPath DatabasePath { get; }
    }
}

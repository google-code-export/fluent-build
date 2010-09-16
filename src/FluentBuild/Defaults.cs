namespace FluentBuild
{
    /// <summary>
    /// Deletes the folder if it exists. If it does not exist then no action is taken
    /// </summary>
    /// <returns>The current BuildFolder</returns>

    public static class Defaults
    {
        public static OnError OnError = FluentBuild.OnError.Fail;
        //TODO: move default framework to here
    }

}
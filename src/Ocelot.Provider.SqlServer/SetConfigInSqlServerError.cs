namespace Ocelot.Provider.SqlServer {
    using Ocelot.Errors;
    public class SetConfigInSqlServerError : Error {
        public SetConfigInSqlServerError(string s)
            : base(s, OcelotErrorCode.UnknownError) {
        }
    }
}

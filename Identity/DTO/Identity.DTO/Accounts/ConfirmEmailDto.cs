namespace Identity.DTO.Accounts
{
    public sealed record ConfirmEmailDto(Guid UserId, string Code);

}


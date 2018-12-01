UPDATE public."MembershipTransactions"
SET "MembershipActionTypeId" = 6
WHERE "Id" in
(SELECT "Id"
  FROM "MembershipTransactions"
 WHERE "MembershipTransactionReferenceId" is not null);
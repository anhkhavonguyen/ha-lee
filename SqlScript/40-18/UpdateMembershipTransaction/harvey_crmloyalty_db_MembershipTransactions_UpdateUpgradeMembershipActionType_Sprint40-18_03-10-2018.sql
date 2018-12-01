UPDATE public."MembershipTransactions"
SET "MembershipActionTypeId" = 2
WHERE "Id" in
(WITH summary AS (
    SELECT "Id", 
           "CustomerId",
			"IPAddress",
			"MembershipTypeId",
	 		"MembershipTransactionReferenceId",
			LAG ("MembershipTypeId", 1) OVER (PARTITION BY "CustomerId" ORDER BY "CreatedDate") as pre_value
      FROM public."MembershipTransactions")
SELECT s."Id"
  FROM summary s
 WHERE s."pre_value" = 1 AND s."MembershipTypeId" = 2 AND s."MembershipTransactionReferenceId" is null);
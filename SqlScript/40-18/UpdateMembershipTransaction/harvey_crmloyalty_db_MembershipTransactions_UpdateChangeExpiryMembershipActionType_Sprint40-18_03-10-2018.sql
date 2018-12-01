UPDATE public."MembershipTransactions"
SET "MembershipActionTypeId" = 7
WHERE "Id" in
(WITH summary AS (
    SELECT "Id", 
           "CustomerId",
			"IPAddress",
			"MembershipTypeId",
	 		"MembershipTransactionReferenceId",
	 		"ExpiredDate",
	 		"CreatedDate",
			LAG ("MembershipTypeId", 1) OVER (PARTITION BY "CustomerId" ORDER BY "CreatedDate") as pre_membership_type,
	 		LAG ("ExpiredDate", 1) OVER (PARTITION BY "CustomerId" ORDER BY "CreatedDate") as pre_expiry_date
      FROM public."MembershipTransactions")
SELECT s."Id"
  FROM summary s
 WHERE s."pre_membership_type" = 2
 	AND s."MembershipTypeId" = 2 
 	AND s."MembershipTransactionReferenceId" is null
	AND s."ExpiredDate" < s."pre_expiry_date");
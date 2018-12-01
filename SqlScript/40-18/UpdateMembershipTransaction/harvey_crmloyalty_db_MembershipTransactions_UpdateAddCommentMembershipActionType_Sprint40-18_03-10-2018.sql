UPDATE public."MembershipTransactions"
SET "MembershipActionTypeId" = 8
WHERE "Id" in
(WITH summary AS (
    SELECT "Id", 
           "CustomerId",
			"IPAddress",
			"MembershipTypeId",
	 		"MembershipTransactionReferenceId",
	 		"ExpiredDate",
	 		"CreatedDate",
			"Comment",
			LAG ("MembershipTypeId", 1) OVER (PARTITION BY "CustomerId" ORDER BY "CreatedDate") as pre_membership_type,
	 		LAG ("ExpiredDate", 1) OVER (PARTITION BY "CustomerId" ORDER BY "CreatedDate") as pre_expiry_date,
			LAG ("Comment", 1) OVER (PARTITION BY "CustomerId" ORDER BY "CreatedDate") as pre_comment
      FROM public."MembershipTransactions")
SELECT s."Id"
  FROM summary s
 WHERE s."pre_membership_type" is not null
	AND s."MembershipTransactionReferenceId" is null
 	AND s."pre_membership_type" = s."MembershipTypeId"
 	AND s."ExpiredDate" = s."pre_expiry_date"
 	AND s."pre_comment" != s."Comment");
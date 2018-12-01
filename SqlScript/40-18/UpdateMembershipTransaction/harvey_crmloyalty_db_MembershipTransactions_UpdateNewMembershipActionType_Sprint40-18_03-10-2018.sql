UPDATE public."MembershipTransactions"
SET "MembershipActionTypeId" = 1
WHERE "Id" in
(WITH summary AS (
    SELECT "Id", 
           "CustomerId",
			"IPAddress",
			"MembershipTypeId",
           ROW_NUMBER() OVER(PARTITION BY "CustomerId"
                                 ORDER BY "CreatedDate") AS rk
      FROM public."MembershipTransactions")
SELECT s."Id"
  FROM summary s
 WHERE (s.rk = 1 AND s."IPAddress" not like 'System Migration' AND s."MembershipTypeId" = 1));
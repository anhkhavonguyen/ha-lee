UPDATE public."AspNetUsers" SET  "UserType"=2 WHERE "IsMigrateData"=true;

UPDATE public."AspNetUsers" AS root
    SET  "NormalizedUserName"="UserName"
    WHERE "IsMigrateData"=true AND NOT "UserName"=ANY(ARRAY(select "UserName"
from public."AspNetUsers"
group by "UserName"
HAVING count(*) > 1))

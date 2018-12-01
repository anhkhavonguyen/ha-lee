INSERT INTO public."AspNetUserRoles"("UserId", "RoleId") SELECT '5d6198e6-d609-4b59-be53-fdf8530f7313', "Id"
	FROM public."AspNetRoles" where "Name" like 'AdminStaff' limit 1
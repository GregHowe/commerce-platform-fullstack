const SITE_CHANGES_SAVED = {
	UserId: "yyy",
	SiteId: "xxx",
	ChangedBlockIds: [],
	Notes: "",
};

const SITE_CHANGES_APPROVED = {
	BrandCode: "",
	ReviewerId: "Compliance Approver Jim",
	SiteId: "xxx",
	Status: "APPROVED",
	Notes: "",
};

const SITE_CHANGES_REJECTED = {
	contentType: "application/json",
	subject: "Compliance-SiteChanges",
	body: {
		PartitionKey: "nyl",
		RowKey: `SITEID-${Date.now()}-${Math.floor(Math.random() * 10000)}`,
		ReviewerId: "yyy",
		SiteId: "xxx",
		Notes: "",
	},
};

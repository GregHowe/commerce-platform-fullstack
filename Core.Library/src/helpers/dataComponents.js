import Mustache from "mustache";

/*
 adding 2 opening brackets causes mustache to error, so we have to make sure both
 closing brackets are there before rendering data
*/
const renderData = (value, object) => {
	if (value) {
		const hasAllBrackets = ["{{", "}}"].every((item) =>
			value.includes(item)
		);
		return hasAllBrackets ? Mustache.render(value, object) : value;
	}
};

/*
 This fetches the site object from the store which we want to pull data from
 depending on builder or generator for prefilling inputs with default values
 using renderData() above.
*/
const sitePath = (store) => {
	if (store?.state?.site) {
		const builderSite = store.state.site.workingSite;
		const generatorSite = store.state.site;
		return builderSite || generatorSite;
	}
};

export { renderData, sitePath };

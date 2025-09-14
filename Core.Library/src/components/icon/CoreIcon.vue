<template>
	<component
		:is="iconName"
		v-bind="$attrs"
		:value="icon"
	/>
</template>

<script>
import {
	kebabCase as _kebabCase,
	camelCase as _camelCase,
	upperFirst as _upperFirst,
	flow as _flow,
} from "lodash";
import iconSchema from "../../schemas/icon.json";
export default {
	name: "CoreIcon",
	props: {
		icon: {
			type: String,
			required: true,
		},
	},
	computed: {
		iconName() {
			return iconSchema.some(
				(icon) => icon.value === _kebabCase(this.icon)
			)
				? "CoreIcon" + _flow(_camelCase, _upperFirst)(this.icon)
				: "CoreIconQuestionMark";
		},
	},
};
</script>

<style></style>

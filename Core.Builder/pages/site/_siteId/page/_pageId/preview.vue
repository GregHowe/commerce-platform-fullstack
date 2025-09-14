<template>
	<div
		:class="pageClasses"
		:style="{
			background: 'var(--_core__body_background)',
			color: 'var(--_core__body_background)',
		}"
	>
		<component
			:is="'style'"
			v-if="selectedThemeColor"
		>
			{{ selectedThemeColor.styles }}
		</component>
		<component
			:is="'style'"
			v-if="selectedThemeFont"
		>
			{{ selectedThemeFont.styles }}
		</component>
		<CoreBlockMainNav :site="workingSite" />
		<CoreBlock
			v-for="(blockSettings, blockIndex) in workingPageBlocks"
			:key="blockSettings.id"
			:index="blockIndex"
			:settings="blockSettings"
		/>
		<CoreBlockFooter :site="workingSite" />
	</div>
</template>

<script>
// there is a 'full screen' view in the builder proper now
// should not be necessary to have multiple views of the same thing to manage like this
import { mapState, mapGetters } from "vuex";
export default {
	layout: "blank",
	data() {
		return {
			pageWidthKeys: ["xs"],
		};
	},
	computed: {
		...mapState("site", ["themeList", "workingSite", "workingPage"]),
		...mapGetters({ hasWorkingChanges: "site/hasWorkingChanges" }),
		workingPageBlocks() {
			return this.workingPage?.blocks || [];
		},
		pageClasses() {
			const classList = ["page"];
			this.pageWidthKeys.forEach((key) => {
				classList.push(`page--${key}`);
			});
			if (!this.workingPageBlocks.length) {
				classList.push("page--empty");
			}
			return classList;
		},
		selectedThemeColor() {
			try {
				const id = this.workingSite?.theme?.color || null;
				return this.themeList?.find((theme) => {
					return theme.id === id;
				});
			} catch (err) {
				console.log(err);
			}
			return "";
		},
		selectedThemeFont() {
			try {
				const id = this.workingSite?.theme?.font || null;
				return this?.themeList?.find((theme) => {
					return theme.id === id;
				});
			} catch (err) {
				console.log(err);
			}
			return "";
		},
	},
};
</script>

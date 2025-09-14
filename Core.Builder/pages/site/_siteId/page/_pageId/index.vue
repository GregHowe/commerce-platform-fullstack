<template>
	<div class="page-id">
		<ModalSiteBuildInProgress />
		<ForgeButton
			v-if="activeView === 'fullscreen'"
			class="button--exit_fullscreen"
			text
			depressed
			tile
			@click="setView(previousView)"
		>
			<v-icon>mdi-fullscreen-exit</v-icon>
		</ForgeButton>

		<ForgeBanner
			page-width-keys="pageWidthKeys"
			:active-view="activeView"
			@view="setView"
		/>
		<SiteSaverMini />
		<div
			ref="page"
			:class="pageClasses"
			:style="{
				background: 'var(--_core__body_background)',
				color: 'var(--_core__body_background)',
			}"
		>
			<NuxtChild :key="$route.params.pageId" />

			<component
				:is="'style'"
				v-if="customStyles"
			>
				{{ customStyles }}
			</component>

			<template v-if="workingPage">
				<template v-if="workingPageBlocks.length">
					<div class="page__body">
						<CoreBlockMainNav
							:site="workingSite"
							:is-builder-mobile="activeView === 'mobile'"
						/>
						<div class="page__blocks">
							<ForgeControlBlock
								v-for="(
									blockSettings, blockIndex
								) in workingPageBlocks"
								:key="blockSettings.id"
								:index="blockIndex"
								:settings="blockSettings"
								:is-first-child="!blockIndex"
								:is-last-child="
									blockIndex >= workingPageBlocks.length - 1
								"
								@select="handleSelect"
								@insert="handleInsert"
								@choose="handleChooseStart"
							/>
						</div>
						<CoreBlockFooter
							:site="workingSite"
							:is-builder-mobile="activeView === 'mobile'"
						/>
					</div>
					<div
						v-if="workingPageBlocks.length"
						class="forge__control-page_unselect"
						@mouseover="clearHighlightedBlock"
						@click="handleSelect([])"
					/>
				</template>
				<template v-else>
					<div>
						<ForgeButton
							text
							@click="handleChooseStart($event, 'Section')"
						>
							Add Section
						</ForgeButton>
						<ForgeButton
							text
							@click="handleChooseStart($event, 'Layout')"
						>
							Add Page Layout
						</ForgeButton>
					</div>
				</template>

				<template v-if="chooserComponent">
					<component
						:is="chooserComponent"
						@choose="handleChooseComplete"
						@close="chooserClose"
					/>
				</template>
			</template>
		</div>
	</div>
</template>

<script>
import blockHelper from "~/helpers/blocks.js";
import { mapState, mapMutations, mapActions, mapGetters } from "vuex";
export default {
	layout: "forge",
	data() {
		return {
			pageWidthKeys: ["xs"],
			activeView: "desktop",
			previousView: "desktop",
			chooserComponent: null,
			chooserPosition: null,
		};
	},
	head() {
		if (this.selectedThemeBase) {
			return {
				link: [
					{
						rel: "stylesheet",
						href: `/themes/${this.selectedThemeBase}.css`,
					},
				],
			};
		}
		return null;
	},
	computed: {
		...mapState("site", ["themeList", "workingSite", "workingPage"]),
		...mapGetters({ hasWorkingChanges: "site/hasWorkingChanges" }),
		customStyles() {
			return this.workingSite?.style;
		},
		pageClasses() {
			const classList = ["page"];
			classList.push(`page--${this.activeView}_view`);
			this.pageWidthKeys.forEach((key) => {
				// so lets pass it in and only track it once
				classList.push(`page--${key}`);
			});
			if (!this.workingPageBlocks.length) {
				classList.push("page--empty");
			}
			return classList;
		},

		workingPageBlocks() {
			return this.workingPage?.blocks || [];
		},
		siteId() {
			return this.$route.params.siteId;
		},
		pageId() {
			return this.$route.params.pageId;
		},
		// selectedThemeColor() {
		// 	try {
		// 		const id = this.workingSite?.themes?.color || null;
		// 		return this.themeList.find((theme) => {
		// 			return theme.id === id;
		// 		});
		// 	} catch (err) {
		// 		console.log(err);
		// 	}
		// 	return null;
		// },
		// selectedThemeFont() {
		// 	try {
		// 		const id = this.workingSite?.themes?.font || null;
		// 		return this.themeList.find((theme) => {
		// 			return theme.id === id;
		// 		});
		// 	} catch (err) {
		// 		console.log(err);
		// 	}
		// 	return null;
		// },
		selectedThemeBase() {
			return this.workingSite.themes?.base || null;
		},
	},
	mounted() {
		window.addEventListener("resize", this.updatePageWidth);
	},
	destroyed() {
		window.removeEventListener("resize", this.updatePageWidth);
	},
	methods: {
		...mapActions("site", [
			"insertBlock",
			"selectBlockIds",
			"saveAndPublishWorkingSite",
		]),
		...mapMutations("site", ["setHighlightedBlockIds"]),
		getBlankBlock(type = null) {
			const block = blockHelper.getBlank();
			block.type = type;
			return block;
		},
		chooserClose() {
			this.chooserPosition = null;
			this.chooserComponent = null;
		},
		clearHighlightedBlock() {
			this.setHighlightedBlockIds();
		},
		handleChooseStart(event, value) {
			this.chooserPosition = [event.position];
			this.chooserComponent = "ForgeChooserSection";
			if (value === "Layout") {
				this.chooserComponent = "ForgeChooserLayout";
			}
		},
		handleChooseComplete(blocks) {
			// here, blocks are CHOSEN from the sections schema
			if (blocks) {
				// insert them backwards so they end up in the right order :D
				for (let idx = blocks.length - 1; idx >= 0; idx--) {
					this.handleInsert({
						settings: blocks[idx],
						position: this.chooserPosition,
					});
				}
			}
			this.chooserClose();
		},
		handleSelect(payload) {
			this.selectBlockIds(payload);
		},
		handleInsert({ settings, position }) {
			this.insertBlock({
				settings,
				position,
			});
		},
		handleInsertPayload(payload) {
			return this.handleInsert(payload); // relay
		},
		setView(key) {
			this.previousView = this.activeView;
			this.activeView = key;
			this.updatePageWidth();
		},
		// todo: fix this thing is not working
		// i think the problem is w/ the generated css
		updatePageWidth() {
			this.$nextTick(() => {
				// your code for handling resize...
				const pageWidth = this.$refs.page
					? this.$refs.page.offsetWidth
					: 0;
				const keys = [];
				// these breakpoints need to stay consistent w/ the scss vars defined in assets/scss/mixins.scss
				if (pageWidth >= 1280) {
					keys.push("xl");
				}
				if (pageWidth >= 1024) {
					keys.push("lg");
				}
				if (pageWidth >= 768) {
					keys.push("md");
				}
				if (pageWidth >= 640) {
					keys.push("sm");
				}
				keys.push("xs");
				this.pageWidthKeys = keys;
			});
		},
	},
};
</script>

<style lang="scss">
.page {
	padding: 1rem;
	min-height: calc(100vh - 128px);
	line-height: 1.5;
	&.page--empty {
		display: flex;
		flex-direction: column;
		align-items: center;
		justify-content: center;
	}
	&.page--mobile_view {
		width: 360px;
		height: 580px;
		min-height: 0;
		justify-content: center;
		display: flex;
		flex-direction: column;
		margin: 120px auto auto;
		//padding: 2rem;
		line-height: 1.5;
		position: relative;
		border-radius: 0.8rem;
		z-index: 3;
		box-shadow: 0.6rem 0.6rem 0.6rem #c0c0c0;
		overflow-x: hidden;
		.page__body {
			position: relative;
			width: 100%;
			min-height: 100%;
			z-index: 109;
			.block {
				height: auto;
			}
		}
	}
	&.page--fullscreen_view {
		position: fixed;
		top: 0;
		right: 0;
		bottom: 0;
		left: 0;
		z-index: 2001;
		overflow: auto;
		/*
			try to make the page act mostly normal,
			but block form submissions for one
			--
			another problem with this approach is the rest of the controls
			are technically still available they are just underneath
			the page now so you can't see them...
			but you might be able to tab into them, or find other ways to interact
			this would be best served in a special layout that removes the interface
		*/
		.page__body {
			pointer-events: all;
		}
		[class*="block"],
		[class^="block"] {
			pointer-events: all;
			cursor: default;
		}
		[class*="forge"],
		[class^="forge"] {
			visibility: hidden;
			pointer-events: none;
		}
		.button[type="submit"] {
			pointer-events: none;
		}
	}
}
// navbar has many issues keep it blocked for now
// .v-main__wrap .page.page--fullscreen_view .navbar {
// 	pointer-events: all;
// }
</style>

<style
	scoped
	lang="scss"
>
.v-main__wrap {
	background-color: grey !important; /* stop doing this // !important */
}
.forge__control-page_unselect {
	position: absolute;
	top: 0;
	right: 0;
	bottom: 0;
	left: 0;
	background: rgba(0, 0, 0, 0);
	z-index: 1;
}
.button_closer {
	position: absolute;
	right: 0;
}
.page__body {
	pointer-events: none; /* blocked in the builder */
}
.views {
	z-index: 2;
}
.button--exit_fullscreen {
	position: fixed;
	top: 1rem;
	right: 1.5rem;
	z-index: 2002; /* just on top of the page */
}
</style>

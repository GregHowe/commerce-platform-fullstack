<template>
	<div class="block__body">
		<CoreBlockContainerToggle
			v-if="hasToggleControls"
			:active-id="activeChildId || ''"
			:settings="settings"
			@set-item="setActiveChild"
		/>

		<CoreBlockContainerTabs
			v-if="showTabs"
			:settings="settings"
			:active-id="activeChildId || ''"
			@set-item="setActiveChild"
		/>

		<CoreBlockContainerAccordion
			v-if="hasAccordionControls"
			:settings="settings"
			:is-showing-contents="isShowingContents"
			@toggle-contents="toggleContents"
		/>

		<div
			v-show="isShowingContents"
			:class="{
				block__section_main: true,
			}"
			:style="blockSectionMainStyles"
		>
			<component
				:is="childBlockComponent"
				v-for="(childSettings, childIndex) in blockChildren"
				:key="childSettings.id"
				:settings="childSettings"
				:index="childIndex"
				:nested-level="nestedLevelNext"
				:is-editable="isEditable"
				:is-first-child="!childIndex"
				:is-last-child="childIndex >= blockChildren.length - 1"
				:parent-is-horizontal="isHorizontal"
				:class="{
					'block__child--active': activeChildId === childSettings.id,
				}"
				@select="selectFromChild"
				@insert="insertFromChild"
			/>
		</div>
		<ul
			v-if="showSlide"
			class="block__control block__control_nav"
		>
			<div
				v-if="isShowingContents && hasSlideControls"
				class="block__control block__control_slide"
				@click="showPreviousChild"
			>
				<CoreIcon icon="carousel-backward" />
			</div>
			<li
				v-for="(childSettings, childIndex) in blockChildren"
				:key="childSettings.id"
				:class="{
					block__control_nav_item: true,
					'block__control_nav_item--active':
						activeChildId === childSettings.id,
				}"
				@click="setActiveChild(childSettings.id)"
			>
				{{ childIndex + 1 }}
			</li>
			<div
				v-if="isShowingContents && hasSlideControls"
				class="block__control block__control_slide"
				@click="showNextChild"
			>
				<CoreIcon icon="carousel-forward" />
			</div>
		</ul>
	</div>
</template>

<script>
import Vue from "vue";
export default {
	name: "CoreBlockContainer",
	props: {
		index: {
			type: Number,
			default: 0,
		},
		nestedLevel: {
			type: Number,
			default: 0,
		},
		settings: {
			type: Object,
			required: true,
		},
		isEditable: {
			type: Boolean,
			default: false,
		},
	},
	data() {
		return {
			activeChildId: null,
			isShowingContents: true,
		};
	},
	computed: {
		activeChildIndex() {
			if (!this.activeChildId) {
				return null;
			}
			return this.blockChildren.findIndex((childSettings) => {
				return childSettings.id === this.activeChildId;
			});
		},
		blockChildren() {
			return this.settings?.blocks || [];
		},
		blockChildrenCount() {
			return this.blockChildren.length;
		},
		blockId() {
			return this.settings.id;
		},
		blockVariantLayout() {
			return this.settings?.variants?.layout || "rows";
		},
		isSlim() {
			return this.settings?.variants["child-width"] === "slim";
		},
		blockChildrenWidth() {
			if (this.isSlim) {
				if(process.client) {
					if (window.matchMedia("(min-width: 768px)").matches) {
						return 33.3;
						console.log("Screen width is at least 768px");
						
					} else {
						//return 100;
						console.log("Screen less than 768px");
						
					}
				}
				
			}
			return 100;
		},
		blockSectionMainStyles() {
			if (this.hasSlideControls) {
				const styles = {
					display: "flex",
					width: `${
						this.blockChildrenCount * this.blockChildrenWidth
					}%`,
					marginLeft: `${
						-this.activeChildIndex * this.blockChildrenWidth
					}%`,
					transition: "margin-left .2s",
					alignItems: "center",
				};
				return styles;
			}
			return null;
		},
		nestedLevelNext() {
			return this.nestedLevel + 1;
		},
		isHorizontal() {
			return (
				["columns", "grid", "carousel", "tabbed", "toggle"].indexOf(
					this.blockVariantLayout
				) >= 0
			);
		},
		/*
			todo the childBlockComponent property is only useful for the builder
			so it should be separated from this block's logic
		*/
		childBlockComponent() {
			if (this.isEditable) {
				return "ForgeControlBlock";
			}
			return "CoreBlock";
		},
		hasPageControls() {
			return (
				["carousel", "tabbed", "toggle"].indexOf(
					this.blockVariantLayout
				) >= 0
			);
		},
		hasSlideControls() {
			return ["carousel"].indexOf(this.blockVariantLayout) >= 0;
		},
		hasAccordionControls() {
			return this.settings.hasAccordion;
		},
		hasToggleControls() {
			return ["toggle"].indexOf(this.blockVariantLayout) >= 0;
		},
		showTabs() {
			return (
				this.isShowingContents &&
				this.hasPageControls &&
				!this.hasSlideControls &&
				!this.hasToggleControls
			);
		},
		showSlide() {
			return (
				this.isShowingContents &&
				this.hasPageControls &&
				this.hasSlideControls
			);
		},
	},
	created() {
		if (this.hasPageControls) {
			if (this.blockChildren.length) {
				// start with the first tab open
				this.setActiveChild(this.blockChildren[0].id);
			}
		}
		if (this.hasSlideControls) {
			// get autoscroll going if selected
			// if (this.settings.autoscroll) {
			// 	console.info("Autoscroll activated!");
			// }
		}
		if (this.hasAccordionControls) {
			// if there is a toggle available, start the contents closed by default
			this.isShowingContents = false;
		}
	},
	/*
		todo separate the builder behaviors below (insert/select)
		from the block's concerns,
		since they only apply in the builder
	*/
	methods: {
		toggleContents() {
			this.isShowingContents = !this.isShowingContents;
		},
		setActiveChild(childId) {
			Vue.set(this, "activeChildId", childId);
			this.selectFromChild([childId]);
		},
		async insertFromChild({ settings, position }) {
			const positionNested = [this.index, ...position];
			this.$emit("insert", { settings, position: positionNested });
			if (this.hasPageControls) {
				await this.$nextTick(); // nextTick to let the event propagate up to the store
				this.setActiveChild(
					this.blockChildren[this.blockChildrenCount - 1].id
				);
			}
		},
		selectFromChild(blockIds = []) {
			const target = this.blockId
				? [this.blockId, ...blockIds]
				: blockIds;
			this.$emit("select", target);
		},
		showNextChild() {
			let newIndex = this.blockChildren.findIndex((childSettings) => {
				return childSettings.id === this.activeChildId;
			});
			newIndex++;
			if (newIndex >= this.blockChildren.length) {
				newIndex = 0;
			}
			if (this.blockChildren[newIndex]) {
				this.setActiveChild(this.blockChildren[newIndex].id);
			}
		},
		showPreviousChild() {
			let newIndex = this.blockChildren.findIndex((childSettings) => {
				return childSettings.id === this.activeChildId;
			});
			newIndex--;
			if (newIndex < 0) {
				newIndex = this.blockChildren.length - 1;
			}
			if (this.blockChildren[newIndex]) {
				this.setActiveChild(this.blockChildren[newIndex].id);
			}
		},
	},
};
</script>

<style lang="scss">
/*
	Do not use utility classes
	or Vuetify components within
	Core Blocks.
	Define all the styles here
	with CSS, using vars from
	the theme where appropriate.
*/
.block_section {
	padding: 0.5rem;
}

.block_section,
.block_accordion,
.block_toggle,
.block_tabs,
.block_container {
	&.block_container_hero {
		overflow: hidden;
		> .block__body {
			> .block__section_main {
				position: relative;
				width: 80%;
				margin: 0 auto;
			}
		}
	}
	&.block_container_rows {
		> .block__body {
			> .block__section_main {
				@include breakpoint_up(
					lg,
					".block_container.block_container_rows > .block__body > .block__section_main"
				) {
					display: block;
					> .block {
						width: 100%;
						flex: 0;
					}
				}
			}
		}
	}
	&.block_section_carousel,
	&.block_container_carousel {
		&.block_container_slim {
			//overflow: hidden;

			.block__control.block__control_slide {
				//display: none;
			}
			.block {
					@include breakpoint_up(lg) {
					& > .block__body > .block__section_main > .block {
					width: calc(var(--_core__container_max-width) / 3);
					&:first-of-type {
						width: 100%;
					}
				}
				}
				
				& > .block__body > .block__section_main > .block {
					width: 100%;
				}
	
			}
		}

		.block__control_slide {
			position: absolute;
			position: relative;
			margin: 0 5px;
			height: 2rem;
			display: flex;
			align-items: center;
			@include breakpoint_up(lg) {
				margin: 0 36px;
			}
		}
		.block__control_nav {
			justify-content: center;
		}
	}
	&.block_section_tabbed,
	&.block_container_tabbed,
	&.block_tabs_tabbed,
	&.block_toggle,
	&.block_container_toggle {
		> .block__body {
			> .block__section_main {
				display: block;
				> .block {
					display: none;
					&.block__child--active {
						display: block;
					}
				}
			}
		}
	}
	&.block_container_columns {
		> .block__body {
			@include breakpoint_up(
				lg,
				".block_container.block_container_columns > .block__body"
			) {
				> .block__section_main {
					display: flex;
					flex-direction: row;
					justify-content: var(--_core__body_justify-content);
					flex-wrap: wrap;
					position: relative;
					margin: 0 auto;
					> .block {
						margin: 0;
						flex: 1;
					}
				}
			}
		}
	}
	&.block_container_grid {
		> .block__body {
			> .block__section_main {
				@include breakpoint_up(
					lg,
					".block_container.block_container_grid > .block__body > .block__section_main"
				) {
					display: flex;
					flex-direction: row;
					flex-wrap: wrap;
					justify-content: space-between;
					& > .block {
						width: 31%; /* should be able to use different num of columns than 3 */
						flex: 1;
					}
				}
			}
		}
		&.block_container_right {
			> .block__body {
				> .block__section_main {
					flex-direction: row-reverse;
				}
			}
		}
		&.block_container_center {
			> .block__body {
				> .block__section_main {
				}
			}
		}
	}
	&.block_container_y-center {
		> .block__body {
			> .block__section_main {
				@include breakpoint_up(
					lg,
					".block_container.block_container_y-center > .block__body > .block__section_main"
				) {
					align-items: center;
				}
			}
		}
	}
	&.block_container_y-bottom {
		> .block__body {
			> .block__section_main {
				@include breakpoint_up(
					lg,
					".block_container.block_container_y-bottom > .block__body > .block__section_main"
				) {
					align-items: flex-end;
				}
			}
		}
	}
	&.block_container_toggle,
	&.block_toggle {
		> .block__body {
			> .block_container_toggle__buttons--container {
				display: flex;
				justify-content: center;
				padding-bottom: 0.6rem;
				pointer-events: all;
				> .block_container_toggle__buttons {
					border: solid 1px var(--core__color_dark);
					border-radius: var(--_core__button_border-radius);
					padding: 0.1rem;
					& button {
						height: 36px;
						min-width: 64px;
						padding: 0 16px;
						font-weight: bold;
						&.active {
							background-color: var(--core__color_primary);
							color: var(--core__color_light-alt);
							&:hover {
								background-color: var(--core__color_primary);
							}
						}
						&:hover {
							background-color: var(--core__color_light);
						}
					}
				}
			}
		}
	}
	.block__control {
		position: relative;
		z-index: 699;
		pointer-events: all; /* they are clickable in the builder interface */
		&.block__control_accordion {
			display: flex;
			align-items: center;
			justify-content: space-between;
			width: 100%;
			padding: 0 0.5rem;
			font-weight: bold;
			font-size: 1rem;
			text-transform: uppercase;
		}
	}
}

.block_container {
	/* constrains background to container width */
	&.block_container_contained > .block__body {
		max-width: var(--_core__container_max-width);
	}
	/* extends contents to full body width */
	&.block_container_full-bleed > .block__body > .block__container {
		max-width: var(--_core__body_max-width);
	}
	& > .block__body {
		background: var(--_core__container_background);
		//overflow: hidden;
	}

	&.block_container_transparent > .block__body {
		--_core__container_background: transparent;

	}

	&.block_container_white > .block__body {
		--_core__container_background: var(--core__color_white);
		--_core__container_color: var(--core__color_primary);
		--_core__header_color: var(--core__color_primary);
		--_core__overline_color: var(--_core__header_color);
		--_core__subline_color: var(--_core__header_color);
		--_core__disclosure_color: var(--_core__header_color);
	}

	&.block_container_light > .block__body {
		--_core__container_background: var(--core__color_tertiary);
		--_core__container_color: var(--core__color_dark);
		--_core__header_color: var(--core__color_primary);
		--_core__overline_color: var(--_core__header_color);
		--_core__subline_color: var(--_core__header_color);
		--_core__disclosure_color: var(--_core__container_color);
	}

	&.block_container_primary > .block__body {
		--_core__container_background: var(--core__color_primary);
		--_core__container_color: var(--core__color_light-alt);
		--_core__header_color: var(--core__color_light);
		--_core__overline_color: var(--_core__header_color);
		--_core__subline_color: var(--_core__header_color);
		--_core__disclosure_color: var(--_core__container_color);
	}

	&.block_container_dark > .block__body {
		--_core__container_background: var(--core__color_secondary--dark);
		--_core__container_color: var(--core__color_light-alt);
		--_core__header_color: var(--core__color_light);
		--_core__overline_color: var(--_core__header_color);
		--_core__subline_color: var(--_core__header_color);
		--_core__disclosure_color: var(--_core__container_color);
	}
}

</style>

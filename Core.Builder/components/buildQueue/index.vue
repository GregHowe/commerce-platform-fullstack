<template>
	<v-card
		max-width="340px"
		class="text-center mt-0 pt-0"
	>
		<v-card-subtitle class="font-weight-bold">{{
			publishText
		}}</v-card-subtitle>
		<v-card-text class="text-center">
			<div class="mb-2">
				<span :class="`${progressTextColor} font-weight-bold mr-3`"
					>Place in line</span
				>
			</div>
			<v-progress-circular
				:size="100"
				:width="15"
				:value="percentComplete"
				:indeterminate="isPublishing"
				:color="progressColor"
			>
				<span class="text-h3">{{ placeInLine }}</span>
			</v-progress-circular>
			<div
				v-show="timeEstimate && !publishFinishing"
				class="pt-2 black--text"
			>
				<v-icon>mdi-clock-outline</v-icon> About
				{{ minutesSeconds }} remaining
			</div>
			<div
				v-show="timeEstimate && publishFinishing"
				class="pt-2 black--text"
			>
				<v-icon>mdi-clock-outline</v-icon> Finishing...
				<v-progress-linear
					v-show="publishFinishing"
					class="mt-2"
					:color="progressColor"
					indeterminate
				/>
			</div>
		</v-card-text>
	</v-card>
</template>
<script>
import { mapState, mapGetters } from "vuex";

function millisToMinutesAndSeconds(millis) {
	var minutes = Math.floor(millis / 60000);
	var seconds = ((millis % 60000) / 1000).toFixed(0);
	return minutes + ":" + (seconds < 10 ? "0" : "") + seconds;
}

export default {
	data() {
		return {
			timer: {},
			timeEstimate: "",
		};
	},

	computed: {
		...mapState({
			queue: (state) => state.buildQueue.queue,
			workingSite: (state) => state.site.workingSite,
		}),
		...mapGetters({
			placeInLine: "buildQueue/placeInLine",
			totalInQueue: "buildQueue/totalInQueue",
			isQueued: "buildQueue/isQueued",
			isPublishing: "buildQueue/isPublishing",
			siteBuild: "buildQueue/siteBuild",
		}),
		percentComplete() {
			return (this.placeInLine / this.totalInQueue) * 100;
		},
		publishText() {
			return this.isPublishing
				? "Your site is now being processed!"
				: "Your site is scheduled to be published!";
		},
		progressColor() {
			return this.isPublishing ? "green" : "blue";
		},
		progressTextColor() {
			return this.isPublishing ? "green--text" : "blue--text";
		},
		siteBuild() {
			return this.queue.find(
				(q) =>
					q?.templateParameters?.CLOUDCMS_SITEID ===
					this.workingSite.id
			);
		},
		publishFinishing() {
			return this.timeEstimate < 5000;
		},
		minutesSeconds() {
			return millisToMinutesAndSeconds(this.timeEstimate);
		},
	},
	mounted() {
		this.timer = setInterval(() => {
			this.timeEstimate =
				360000 * this.placeInLine +
				1 -
				Math.abs(new Date(this.siteBuild?.createdDate) - new Date());
		}, 1000);
	},
	beforeDestroy() {
		clearInterval(this.timer);
	},
};
</script>
<style
	lang="scss"
	scoped
>
.description {
	font-size: 16px;
	line-height: 21px;
}
</style>

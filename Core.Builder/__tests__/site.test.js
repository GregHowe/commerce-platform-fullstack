import Vuex from "vuex";
import { createLocalVue } from "@vue/test-utils";
import { getters, mutations } from "~/store/site";

const localVue = createLocalVue();
localVue.use(Vuex);

describe("site store pages", () => {
	test("setting stored site also sets working pages", () => {
		mutations.setStoredSite(mockState, mockSite);
		mutations.cloneStoredSite(mockState);
		expect(mockState.workingSite.pages.length).toEqual(
			mockSite.pages.length
		);
		// no working changes detected
		expect(getters.hasWorkingChanges(mockState)).toBe(false);
	});

	test("cannot set a page id which does not exist", () => {
		mutations.setSelectedPageId(mockState, "page99");
		expect(mockState.selectedPageId).toEqual(null);
	});

	test("changing working page does not mutate stored page and resets back to previous state", () => {
		mutations.setStoredSite(mockState, mockSite);
		mutations.cloneStoredSite(mockState);
		const firstWorkingPage = mockState.workingSite.pages[0];
		// seo title is undefined here
		expect(firstWorkingPage.seo.title).toBe(undefined);
		const firstStoredPage = mockState.storedSite.pages[0];
		// set working Page
		mutations.setSelectedPageId(mockState, firstWorkingPage.id);

		// change the value of seo title
		const newValue = "new test seo title";
		mutations.setWorkingPageSetting(mockState, {
			key: "seo.title",
			value: newValue,
		});
		// seo title is set to new working value in the array of pages and the working page
		expect(mockState.workingPage.seo.title).toEqual(newValue);
		expect(firstWorkingPage.seo.title).toBe(newValue);
		// the stored page still has its original value
		expect(firstStoredPage.seo.title).not.toEqual(
			firstWorkingPage.seo.title
		);
		expect(firstStoredPage.seo.title).toBe(undefined);

		// detects working changes
		// stored and working pages reset back
		expect(getters.hasWorkingChanges(mockState)).toBe(true);

		mutations.cloneStoredSite(mockState);
		mutations.resetWorkingPageAndBlock(mockState);
		expect(mockState.workingSite.pages[0].seo.title).toBe(undefined);
		expect(mockState.storedSite.pages[0].seo.title).toBe(undefined);
		// working page resets back
		expect(mockState.workingPage.seo.title).toBe(undefined);
		expect(getters.hasWorkingChanges(mockState)).toBe(false);
	});

	test("changing working block does not mutate stored block and resets back to previous state", () => {
		mutations.setStoredSite(mockState, mockSite);
		mutations.cloneStoredSite(mockState);
		const firstWorkingPage = mockState.workingSite.pages[0];
		const firstStoredPage = mockState.storedSite.pages[0];
		// set working Page
		mutations.setSelectedPageId(mockState, firstWorkingPage.id);

		// set working block
		mutations.setSelectedBlockIds(mockState, [
			mockState.workingPage.blocks[0].id,
		]);

		// change the value of the block
		const newValue = "some donuts";
		mutations.setWorkingBlockSetting(mockState, {
			key: "content",
			value: newValue,
		});

		// block is set to new value
		expect(mockState.workingBlock.content).toEqual(newValue);
		expect(mockState.workingPage.blocks[0].content).toBe(newValue);

		// the stored page still has its original value
		expect(firstStoredPage.blocks[0].content).not.toEqual(newValue);
		expect(firstStoredPage.blocks[0].content).toBe(undefined);

		// detects working changes
		expect(getters.hasWorkingChanges(mockState)).toBe(true);

		// workingBlock inside page arrays reset back
		mutations.cloneStoredSite(mockState);
		mutations.resetWorkingPageAndBlock(mockState);
		expect(mockState.workingSite.pages[0].blocks[0].content).toBe(
			undefined
		);
		expect(mockState.storedSite.pages[0].blocks[0].content).toBe(undefined);

		// workingBlock resets back
		expect(mockState.workingBlock).toEqual(
			mockState.storedSite.pages[0].blocks[0]
		);
		expect(getters.hasWorkingChanges(mockState)).toBe(false);
	});
});

describe("site store site", () => {
	test("setting stored site also sets working site", () => {
		mutations.setStoredSite(mockState, mockSite);
		mutations.cloneStoredSite(mockState);
		expect(mockState.workingSite).toEqual(mockSite);
		expect(mockState.storedSite).toEqual(mockSite);

		// no working changes detected
		expect(getters.hasWorkingChanges(mockState)).toBe(false);
	});

	test("changing working site does not mutate stored site", () => {
		// set stored site to the mock site
		mutations.setStoredSite(mockState, mockSite);
		mutations.cloneStoredSite(mockState);

		// change the value of a nested property in the working site
		const newValue = "new test seo title";
		mutations.setWorkingSiteSetting(mockState, {
			key: "seo.title",
			value: newValue,
		});
		expect(mockState.workingSite.seo.title).toEqual(newValue);
		expect(mockState.workingSite.seo.title).not.toEqual(mockSite.seo.title);

		// stored site still only contains the original value
		expect(mockState.storedSite.seo.title).toEqual(mockSite.seo.title);

		// detect working changes
		expect(getters.hasWorkingChanges(mockState)).toBe(true);
	});
});

const mockSite = {
	title: "Test Site",
	seo: {
		title: "test seo title",
	},
	pages: [
		{
			id: "page1", // 5 levels of blocks
			seo: {},
			blocks: [
				{ id: "level0_text", type: "text" },
				{
					id: "level0_container",
					type: "container",
					blocks: [
						{ id: "level1_text", type: "text" },
						{
							id: "level1_container",
							type: "container",
							blocks: [
								{ id: "level2_text", type: "text" },
								{
									id: "level2_container",
									type: "container",
									blocks: [
										{ id: "level3_text", type: "text" },
										{
											id: "level3_container",
											type: "container",
											blocks: [
												{
													id: "level4_text",
													type: "text",
												},
												{
													id: "level4_container",
													type: "container",
													blocks: [
														{
															id: "level5_text",
															type: "text",
														},
														{
															id: "level5_container",
															type: "container",
															blocks: [],
														},
													],
												},
											],
										},
									],
								},
							],
						},
					],
				},
			],
		},
		{
			id: "page2", // no blocks
			blocks: [],
		},
	],
};

const mockState = {
	selectedPageId: null,
	workingPage: {},
	workingSite: {},
	storedSite: null,
	workingBlock: null,
	selectedBlockIds: [],
	selectedBlockId: null,
};

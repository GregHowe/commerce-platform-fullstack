function _getBlankBlock() {
	return {
		id: _generateRandomBlockId(),
		type: "",
	};
}

function _generateRandomBlockId(length = 6) {
	const letters = "bcdfghjkmnpqrstvwxyz";
	const characters = letters + "23456789";
	let id = letters[Math.floor(Math.random() * letters.length)];
	for (let i = 1; i < length; i++) {
		id += characters[Math.floor(Math.random() * characters.length)];
	}
	return id;
}

function _insertBefore(blocks, blockIndex, newBlock) {
	if (blockIndex === 0) {
		blocks.unshift(newBlock);
	} else {
		blocks.splice(blockIndex, 0, newBlock);
	}
	return blocks;
}
function _insertAfter(blocks, blockIndex, newBlock) {
	if (blockIndex === blocks.length - 1) {
		blocks.push(newBlock);
	} else {
		blocks.splice(blockIndex + 1, 0, newBlock);
	}
	return blocks;
}

function _movePrevious(blocks, blockIndex) {
	if (blockIndex > 0) {
		const blockToMove = blocks.splice(blockIndex, 1)[0];
		blocks.splice(blockIndex - 1, 0, blockToMove);
	}
	return blocks;
}
function _moveNext(blocks, blockIndex) {
	if (blockIndex < blocks.length - 1) {
		const blockToMove = blocks.splice(blockIndex, 1)[0];
		blocks.splice(blockIndex + 1, 0, blockToMove);
	}
	return blocks;
}

export default ({ app }, inject) => {
	inject("blocks", {
		find: (blocks, id) => {
			if (id) {
				const path = app.$util.findPath(blocks, "id", id);
				return app.$util.getPath(blocks, path);
			}
			return null;
		},
		findPath: (blocks, id) => {
			if (id) {
				return app.$util.findPath(blocks, "id", id);
			}
			return null;
		},
		insert: (newBlock, blocks, blockIndex, placement) => {
			if (placement === "before") {
				return {
					blocks: _insertBefore(blocks, blockIndex, newBlock),
					newBlock,
				};
			} else if (placement === "after") {
				return {
					blocks: _insertAfter(blocks, blockIndex, newBlock),
					newBlock,
				};
			}
		},
		insertEmpty: (blocks, blockIndex, placement) => {
			const newBlock = _getBlankBlock();
			if (placement === "before") {
				return {
					blocks: _insertBefore(blocks, blockIndex, newBlock),
					newBlock: newBlock,
				};
			} else if (placement === "after") {
				return {
					blocks: _insertAfter(blocks, blockIndex, newBlock),
					newBlock: newBlock,
				};
			}
		},
		move: (blocks, blockIndex, direction) => {
			if (direction === "previous") {
				return _movePrevious(blocks, blockIndex);
			} else if (direction === "next") {
				return _moveNext(blocks, blockIndex);
			}
		},
		remove: (blocks, blockIndex) => {
			blocks.splice(blockIndex, 1);
			return blocks;
		},
	});
};

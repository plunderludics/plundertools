local DIR = ".\\Assets\\StreamingAssets\\"
-- local DIR = "./plunder_Data/StreamingAssets"
local lib = require(DIR.."scripts/lib/lib")
local plunder = require(DIR.."scripts/lib/plunder")

plunder.USE_SERVER = true
plunder.USE_SERVER_INPUT = true

frameTimer = 0
SHOW_DEBUG = true

-- this is used externally to send information to this window
windowName = "untitled"

-- if there's multiple instances with same name, they can be used to distinguish them
instance = 0

local currSample
local isPaused

function main()
	print("starting lua stuff")
	-- loop forever waiting for clients
	print("starting rom: ", gameinfo.getromname())
	domainToCheck = "RDRAM"
	currMem = {}

	-- TODO: maybe wait for first load
	-- TODO: maybe send loading state

	-- https://github.com/TASEmulators/BizHawk/issues/1141#issuecomment-410577001
	started = false
	while true do
		gui.clearGraphics()
		local f = emu.framecount()

		emu.frameadvance()
		emu.yield()

	-- end
	-- while true do

		if frameTimer >= 0 then
			frameTimer = frameTimer + 1
		end
		if frameTimer > 60 then
			frameTimer = 0
		end

		-- prevent mario from getting frozen in dialogue
		if (currSample == "m64-courtyard") then
			if (plunder.getValue({byte = 0x33d480, size = 4}) ~= 0) then
				-- mash A
				joypad.set({
					["A"] = f % 10 < 5
				}, 1)
			end
		end

		if SHOW_DEBUG then
			gui.text(10, 10, dbg)
		end
	end
end


main()
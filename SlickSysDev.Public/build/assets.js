const fs = require('fs-extra')
const path = require('path')
const cfg = require('./config')
const configureLogger = require('./logger')

const log = configureLogger('Assets')

async function copyDir(src, dest) {
  try {
    await fs.ensureDir(dest)
    await fs.copy(src, dest, { overwrite: true, errorOnExist: false })
    log.success(`Copied ${src} -> ${dest}`)
  } catch (err) {
    log.error('', `Failed to copy ${src} -> ${dest}: ${err.message}`)
    process.exitCode = 1
  }
}

async function copyFile(src, dest) {
  try {
    await fs.ensureDir(path.dirname(dest))
    await fs.copy(src, dest, { overwrite: true, errorOnExist: false })
    log.success(`Copied ${src} -> ${dest}`)
  } catch (err) {
    log.error('', `Failed to copy ${src} -> ${dest}: ${err.message}`)
    process.exitCode = 1
  }
}

;(async () => {
  await copyDir(cfg.imagesSrc, cfg.images)
  await copyDir('wwwroot/assets/favicon', 'wwwroot/favicon')
  await copyFile('wwwroot/assets/js/theme-switcher.js', 'wwwroot/js/theme-switcher.js')
})()
